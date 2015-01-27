using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Serialization;
using System.IO;
using System.Reflection;
using System.Workflow.Activities.Rules.Design;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.ComponentModel;
using System.Collections;
using System.Security;
using System.Security.Principal;
using System.Security.Permissions;

using Portal.RuleSet;
using Portal.Model.Rules;
using Portal.Data.Sql.EntityFramework.Rules;


namespace Portal.RuleSet.UI
{
    public partial class RuleSetToolkitEditor : Form
    {

        #region Variables and constructor

        private const int maxMinorVersions = 100;
        private const int maxMajorVersions = 1000;
        private readonly WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();

        private RuleSetData selectedRuleSetData;
        private readonly List<RuleSetData> deletedRuleSetDataCollection = new List<RuleSetData>();
        private readonly Dictionary<TreeNode, RuleSetData> ruleSetDataDictionary = new Dictionary<TreeNode, RuleSetData>();
        private bool dirty; //indicates if any RuleSetData has been modified
        private string assemblyPath; //used to resolve assembly errors
        private readonly RulesRepository _rulesRepository = new RulesRepository();

        public RuleSetToolkitEditor()
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            InitializeComponent();
        }

        #endregion

        #region Form level

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        private void Form1_Load(object sender, EventArgs e)
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += ruleSetEditor_AssemblyResolve;
            FormClosing += RuleSetEditor_FormClosing;

            menuStrip1.ItemClicked += menuStrip1_ItemClicked;

            treeView1.TreeViewNodeSorter = new TreeSortClass();
            treeView1.HideSelection = false;

            ruleSetNameBox.Validating += ruleSetNameBox_Validating;
            minorVersionBox.Validating += minorVersionBox_Validating;
            majorVersionBox.Validating += majorVersionBox_Validating;

            majorVersionBox.Maximum = maxMajorVersions;
            minorVersionBox.Maximum = maxMinorVersions;
        }

        void RuleSetEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !ContinueRuleDefinitionsChange();
        }

        //If an assembly referenced by the selected assembly cannot be found try loading it from the same directory as the referenced assembly
        //should only occur when loading a RuleSetData with existing activity information
        Assembly ruleSetEditor_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (selectedRuleSetData != null && !String.IsNullOrEmpty(assemblyPath))
            {
                return ResolveAssembly(assemblyPath, args.Name);
            }
            return null;
        }

        internal static Assembly ResolveAssembly(string assemblyPath, string failedAssemblyName)
        {
            try
            {
                string assemblyName;
                if (failedAssemblyName.Contains(",")) //strong name; need to strip off everything but the name
                {
                    assemblyName = failedAssemblyName.Substring(0, failedAssemblyName.IndexOf(",", StringComparison.Ordinal));
                }
                else
                {
                    assemblyName = failedAssemblyName;
                }
                var tempPath = Path.HasExtension(assemblyPath) ? Path.GetDirectoryName(assemblyPath) : assemblyPath;
                var assemblyPathToTry = tempPath + Path.DirectorySeparatorChar + assemblyName + ".dll";

                var assemblyFileInfo = new FileInfo(assemblyPathToTry);
                if (assemblyFileInfo.Exists)
                {
                    return Assembly.LoadFile(assemblyPathToTry);
                }
                return null;
            }
            catch (Exception) //no luck in resolving the assembly
            {
                return null;
            }
        }

        #endregion

        #region Menu items

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ContinueRuleDefinitionsChange())
            {
                selectedRuleSetData = null;
                var ruleSetDataCollection = GetRuleSets();
                BuildTree(ruleSetDataCollection);

                EnableApplicationFields(true);
                EnableRuleSetFields(false);
            }
        }

        private List<RuleSetData> GetRuleSets()
        {
            dirty = false;

            var ruleSets = _rulesRepository.GetAll<Ruleset>()
                                .OrderBy(r => r.Name)
                                .ThenBy(r => r.MajorVersion)
                                .ThenBy(r => r.MinorVersion)
                                .ToList();

            return ruleSets.Select(ruleSet => new RuleSetData()
                        {
                            Name = ruleSet.Name,
                            OriginalName = ruleSet.Name,
                            MajorVersion = ruleSet.MajorVersion,
                            OriginalMajorVersion = ruleSet.MajorVersion,
                            MinorVersion = ruleSet.MinorVersion,
                            OriginalMinorVersion = ruleSet.MinorVersion,
                            RuleSetDefinition = ruleSet.RuleSetDefinition,
                            Status = ruleSet.Status ?? 1,
                            AssemblyPath = ruleSet.AssemblyPath,
                            ActivityName = ruleSet.ActivityName,
                            ModifiedDate = ruleSet.ModifiedDate ?? DateTime.Now,
                            Dirty = false
                        }).ToList();
        }

        private void BuildTree(List<RuleSetData> ruleSetDataCollection)
        {
            ruleSetDataCollection.Sort();
            ruleSetDataDictionary.Clear();
            treeView1.Nodes.Clear();
            RuleSetData lastData = null;
            TreeNode lastRuleSetNameNode = null;
            foreach (var data in ruleSetDataCollection)
            {
                if (lastData == null || lastData.Name != data.Name) //new ruleset name
                {
                    var newNode = new TreeNode(data.Name);
                    treeView1.Nodes.Add(newNode);
                    lastRuleSetNameNode = newNode;
                }

                var newVersionNode = new TreeNode(VersionTreeNodeText(data.MajorVersion, data.MinorVersion));
                lastRuleSetNameNode.Nodes.Add(newVersionNode);
                ruleSetDataDictionary.Add(newVersionNode, data);

                lastData = data;
            }
            treeView1.Sort();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            try
            {
                SaveToDB();
            }
            catch (SecurityException ex)
            {
                MessageBox.Show(string.Format("({0}) is not authorized to save changes. \n\n {1}", wp.Identity.Name, ex.ToString()));
            }
        }

        private void SaveToDB()
        {
            if (ruleSetDataDictionary == null)
            {
                MessageBox.Show("RuleSet collection is empty.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var dirtyRSDs = new List<RuleSetData>();

            foreach (var data in deletedRuleSetDataCollection.Where(d => !string.IsNullOrEmpty(d.OriginalName)))
            {
                var ruleSet = _rulesRepository.FindBy<Ruleset>(r => r.Name == data.OriginalName && r.MajorVersion == data.OriginalMajorVersion && r.MinorVersion == data.OriginalMinorVersion).FirstOrDefault();

                if (ruleSet != null)
                {
                    var history = new RulesetHistory()
                    {
                        Name = ruleSet.Name,
                        MajorVersion = ruleSet.MajorVersion,
                        MinorVersion = ruleSet.MinorVersion,
                        RuleSet = ruleSet.RuleSetDefinition,
                        Status = ruleSet.Status,
                        AssemblyPath = ruleSet.AssemblyPath,
                        ActivityName = ruleSet.ActivityName,
                        ModifiedBy = ruleSet.ModifiedBy,
                        ModifiedDate = DateTime.Now
                    };

                    _rulesRepository.Add(history);
                    _rulesRepository.Delete(ruleSet);
                    _rulesRepository.Save();
                }
            }

            foreach (var data in ruleSetDataDictionary.Values.Where(d => d.Dirty))
            {
                dirtyRSDs.Add(data);
                data.RuleSetDefinition = SerializeRuleSet(data.RuleSet);

                var ruleSet = _rulesRepository.FindBy<Ruleset>(r => r.Name == data.Name && r.MajorVersion == data.MajorVersion && r.MinorVersion == data.MinorVersion).FirstOrDefault();

                if (ruleSet != null)
                {
                    var history = new RulesetHistory()
                    {
                        Name = ruleSet.Name,
                        MajorVersion = ruleSet.MajorVersion,
                        MinorVersion = ruleSet.MinorVersion,
                        RuleSet = ruleSet.RuleSetDefinition,
                        Status = ruleSet.Status,
                        AssemblyPath = ruleSet.AssemblyPath,
                        ActivityName = ruleSet.ActivityName,
                        ModifiedBy = ruleSet.ModifiedBy,
                        ModifiedDate = DateTime.Now
                    };

                    _rulesRepository.Add(history);

                    ruleSet.Name = data.Name;
                    ruleSet.MajorVersion = data.MajorVersion;
                    ruleSet.MinorVersion = data.MinorVersion;
                    ruleSet.RuleSetDefinition = data.RuleSetDefinition;
                    ruleSet.Status = data.Status;
                    ruleSet.AssemblyPath = data.AssemblyPath;
                    ruleSet.ActivityName = data.ActivityName;
                    ruleSet.ModifiedBy = data.ModifiedBy;
                    ruleSet.ModifiedDate = DateTime.Now;

                    _rulesRepository.Update(ruleSet);
                    _rulesRepository.Save();
                }
                else
                {
                    ruleSet = new Ruleset()
                    {
                        Name = data.Name,
                        MajorVersion = data.MajorVersion,
                        MinorVersion = data.MinorVersion,
                        RuleSetDefinition = data.RuleSetDefinition,
                        Status = data.Status,
                        AssemblyPath = data.AssemblyPath,
                        ActivityName = data.ActivityName,
                        ModifiedBy = data.ModifiedBy,
                        ModifiedDate = DateTime.Now,
                    };

                    _rulesRepository.Add(ruleSet);
                    _rulesRepository.Save();
                }
            }

            // After updates have been stored to the DB, set/reset the "Original" values
            foreach (var data in dirtyRSDs)
            {
                data.OriginalName = data.Name;
                data.OriginalMajorVersion = data.MajorVersion;
                data.OriginalMinorVersion = data.MinorVersion;
                data.Dirty = false;
            }

            deletedRuleSetDataCollection.Clear();

            dirty = false;
        }

        private string SerializeRuleSet(System.Workflow.Activities.Rules.RuleSet ruleSet)
        {
            var ruleDefinition = new StringBuilder();

            if (ruleSet != null)
            {
                try
                {
                    var stringWriter = new StringWriter(ruleDefinition, CultureInfo.InvariantCulture);
                    var writer = new XmlTextWriter(stringWriter);
                    serializer.Serialize(writer, ruleSet);
                    writer.Flush();
                    writer.Close();
                    stringWriter.Flush();
                    stringWriter.Close();
                }
                catch (Exception ex)
                {
                    if (selectedRuleSetData != null)
                        MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "Error serializing RuleSet: '{0}'. \r\n\n{1}", selectedRuleSetData.Name, ex.Message), "Serialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "Error serializing RuleSet. \r\n\n{0}", ex.Message), "Serialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (selectedRuleSetData != null)
                    MessageBox.Show(String.Format(CultureInfo.InvariantCulture, "Error serializing RuleSet: '{0}'.", selectedRuleSetData.Name), "Serialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Error serializing RuleSet.", "Serialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ruleDefinition.ToString();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool ContinueRuleDefinitionsChange()
        {
            var continueResult = true;

            if (dirty)
            {
                var result = MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "Do you want to save the changes?"), "RuleSet Editor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(null, EventArgs.Empty);
                }
                else if (result == DialogResult.No)
                {
                }
                else //Cancel
                {
                    continueResult = false;
                }
            }
            return continueResult;
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Rules files (*.rules)|*.rules";
            var fileResult = fileDialog.ShowDialog();

            if (fileResult == DialogResult.OK && !String.IsNullOrEmpty(fileDialog.FileName))
            {
                var reader = new XmlTextReader(fileDialog.FileName);
                object objectRuleSet = null; ;

                try
                {
                    objectRuleSet = serializer.Deserialize(reader);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(CultureInfo.InvariantCulture, "Error loading file.  \r\n\n{0}", ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                reader.Close();

                var ruleDefinitions = objectRuleSet as RuleDefinitions;

                if (ruleDefinitions != null)
                {
                    if (ruleDefinitions.RuleSets.Count > 0)
                    {
                        RuleSetData ruleSetData = null;
                        var ruleSetDataCollection = new List<RuleSetData>();
                        foreach (System.Workflow.Activities.Rules.RuleSet ruleSet in ruleDefinitions.RuleSets)
                        {
                            ruleSetData = CreateRuleSetData(ruleSet);

                            //find the next available major version
                            ruleSetData.MajorVersion = GetNextMajorVersion(ruleSet.Name);
                            ruleSetDataCollection.Add(ruleSetData);
                        }

                        var selectorForm = new RuleSetSelector();
                        selectorForm.RuleSetDataCollection.AddRange(ruleSetDataCollection);
                        selectorForm.SelectAll = true;
                        selectorForm.Instructions = "Select the RuleSets you would like to import.  Each RuleSet has been assigned the next available Major Version number.";
                        var selectorResult = selectorForm.ShowDialog();

                        if (selectorResult == DialogResult.OK && selectorForm.SelectedRuleSetDataCollection != null)
                        {
                            foreach (var data in selectorForm.SelectedRuleSetDataCollection)
                            {
                                AddRuleSetData(data);
                                GetThisType(Path.GetDirectoryName(fileDialog.FileName), Path.GetFileName(fileDialog.FileName));
                            }
                            treeView1_AfterSelect(this, new TreeViewEventArgs(treeView1.SelectedNode));  //force this call so that assembly and activity information if populated on form
                        }
                    }
                    else
                    {
                        MessageBox.Show("File does not contain any RuleSets.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("File is not a valid .rules file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectorForm = new RuleSetSelector();
            selectorForm.RuleSetDataCollection.AddRange(ruleSetDataDictionary.Values);
            selectorForm.Instructions = "Select the RuleSets you would like to export.  Note that version numbers are not included in the output file, and only a single RuleSet with a given Name can be exported to the same file.";
            var selectorResult = selectorForm.ShowDialog();

            if (selectorResult == DialogResult.OK && selectorForm.SelectedRuleSetDataCollection.Count > 0)
            {
                var ruleDefinitions = new RuleDefinitions();
                foreach (var data in selectorForm.SelectedRuleSetDataCollection)
                {
                    ruleDefinitions.RuleSets.Add(data.RuleSet);
                }

                var dialog = new SaveFileDialog();
                dialog.Filter = "Rules files (*.rules)|*.rules";
                dialog.AddExtension = true;
                dialog.DefaultExt = "rules";
                var result = dialog.ShowDialog();

                if (result == DialogResult.OK && !String.IsNullOrEmpty(dialog.FileName))
                {
                    var writer = new XmlTextWriter(dialog.FileName, null);
                    serializer.Serialize(writer, ruleDefinitions);
                    writer.Flush();
                    writer.Close();
                }
            }
            else
            {
                MessageBox.Show("No RuleSets selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void validateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedRuleSetData != null)
            {
                if (ActivitySelector.ValidateRuleSet(selectedRuleSetData.RuleSet, selectedRuleSetData.Activity, false))
                    MessageBox.Show("RuleSet is valid.", "RuleSet Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region TreeView

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RuleSetData data;
            if (e.Node != null && ruleSetDataDictionary.TryGetValue(e.Node, out data))
            {
                selectedRuleSetData = data;
                assemblyPath = selectedRuleSetData.AssemblyPath;
                ruleSetNameBox.Text = selectedRuleSetData.Name;
                majorVersionBox.Value = selectedRuleSetData.MajorVersion;
                minorVersionBox.Value = selectedRuleSetData.MinorVersion;
                activityBox.Text = selectedRuleSetData.ActivityName;

                if (selectedRuleSetData.Activity == null)
                    LoadAssemblyAndActivity();

                PopulateMembers();

                EnableRuleSetFields(true);
            }
            else
            {
                selectedRuleSetData = null;
                assemblyPath = null;
                EnableRuleSetFields(false);
            }
        }

        private void SetSelectedNode(TreeNode node)
        {
            if (node != null)
            {
                treeView1.SelectedNode = node;
                treeView1_AfterSelect(this, new TreeViewEventArgs(node));
            }
            else
            {
                treeView1.SelectedNode = null;
                treeView1_AfterSelect(this, new TreeViewEventArgs(null));
            }
        }

        private TreeNode FindParentNode(RuleSetData data)
        {
            if (data != null)
            {
                foreach (TreeNode node in treeView1.Nodes)
                {
                    if (String.CompareOrdinal(node.Text, data.Name) == 0)
                        return node;
                }
            }
            return null;
        }

        private void EnableApplicationFields(bool enable)
        {
            newButton.Enabled = enable;
            ruleSetNameCollectionLabel.Enabled = enable;

            if (!enable)
                EnableRuleSetFields(enable);
        }

        private void EnableRuleSetFields(bool enable)
        {
            editButton.Enabled = enable;
            deleteButton.Enabled = enable;
            copyButton.Enabled = enable;
            ruleSetNameBox.Enabled = enable;
            ruleSetNameLabel.Enabled = enable;
            majorVersionBox.Enabled = enable;
            majorVersionLabel.Enabled = enable;
            minorVersionBox.Enabled = enable;
            minorVersionLabel.Enabled = enable;
            getActivityButton.Enabled = enable;
            selectedActivityLabel.Enabled = enable;
            membersLabel.Enabled = enable;
            validateToolStripMenuItem.Enabled = enable;

            if (!enable)
                ClearRuleSetFields();
        }

        private void ClearRuleSetFields()
        {
            ruleSetNameBox.Text = "";
            majorVersionBox.Value = 0;
            minorVersionBox.Value = 0;
            activityBox.Text = "";
            membersBox.Items.Clear();
        }

        private void LoadAssemblyAndActivity()
        {
            if (selectedRuleSetData != null)
            {
                activityBox.Text = "";
                membersBox.Items.Clear();

                if (!String.IsNullOrEmpty(assemblyPath) && !String.IsNullOrEmpty(selectedRuleSetData.ActivityName))
                {
                    var assembly = LoadAssembly(assemblyPath);

                    if (assembly != null)
                    {
                        var activityType = LoadActivity(assembly, selectedRuleSetData.ActivityName);
                        if (activityType != null)
                        {
                            activityBox.Text = activityType.ToString();
                            selectedRuleSetData.Activity = activityType;
                            PopulateMembers();
                        }
                    }
                }
            }
        }

        internal static Assembly LoadAssembly(string assemblyPath)
        {
            Assembly assembly = null;
            if (!String.IsNullOrEmpty(assemblyPath))
            {
                try
                {
                    var assemblyFileInfo = new FileInfo(assemblyPath);
                    if (assemblyFileInfo.Exists)
                    {
                        assembly = Assembly.LoadFile(assemblyPath);
                    }
                    else
                    {
                        //try to load from the application directory
                        var assemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(assemblyPath));
                        assembly = Assembly.Load(assemblyName);
                    }
                }
                catch (FileLoadException ex)
                {
                    MessageBox.Show(String.Format(CultureInfo.InvariantCulture, "Error loading assembly for the referenced Type at: \r\n\n'{0}' \r\n\n{1}", assemblyPath, ex.Message), "Assembly Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show(String.Format(CultureInfo.InvariantCulture, "Error loading assembly for the referenced Type at: \r\n\n'{0}'", assemblyPath), "Assembly Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return assembly;
        }

        internal static Type LoadActivity(Assembly assembly, string activityName)
        {
            Type activityType = null;
            if (assembly != null && !String.IsNullOrEmpty(activityName))
            {
                try
                {
                    activityType = assembly.GetType(activityName, false);
                }
                catch (TypeLoadException ex)
                {
                    MessageBox.Show(String.Format(CultureInfo.InvariantCulture, "Error loading the target Type from the assembly: \r\n\n{0}", ex.Message), "Type Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    MessageBox.Show(String.Format(CultureInfo.InvariantCulture, "Error loading the target Type from the assembly: \r\n\n{0}", ex.LoaderExceptions[0].Message), "Type Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return activityType;
        }

        #endregion

        #region ActivitySelector

        private void getActivityButton_Click(object sender, EventArgs e)
        {
            var activitySelector = new ActivitySelector();

            if (selectedRuleSetData != null)
            {
                activitySelector.AssemblyPath = selectedRuleSetData.AssemblyPath;
                activitySelector.Activity = selectedRuleSetData.Activity;
                activitySelector.RuleSet = selectedRuleSetData.RuleSet;
            }

            activitySelector.ShowDialog();

            if (selectedRuleSetData != null && !String.IsNullOrEmpty(activitySelector.AssemblyPath) && activitySelector.Activity != null)
            {
                if (string.CompareOrdinal(selectedRuleSetData.AssemblyPath, activitySelector.AssemblyPath) != 0 || selectedRuleSetData.Activity != activitySelector.Activity)
                {
                    selectedRuleSetData.AssemblyPath = activitySelector.AssemblyPath;
                    selectedRuleSetData.Activity = activitySelector.Activity;
                    activityBox.Text = activitySelector.Activity.ToString();
                    PopulateMembers();
                    MarkDirty(selectedRuleSetData);
                }
            }
        }

        private void PopulateMembers()
        {
            if (selectedRuleSetData != null)
            {
                membersBox.Items.Clear();

                var activityType = selectedRuleSetData.Activity;

                if (activityType != null)
                {
                    membersBox.Items.AddRange(GetMembers(activityType).ToArray());
                }
            }
        }

        internal static List<string> GetMembers(Type targetType)
        {
            var members = new List<string>();

            if (targetType != null)
            {
                try
                {
                    var properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var property in properties)
                    {
                        members.Add(string.Format(CultureInfo.InvariantCulture, "{0}   ({1})", property.Name, property.PropertyType));
                    }

                    var fields = targetType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var field in fields)
                    {
                        members.Add(string.Format(CultureInfo.InvariantCulture, "{0}   ({1})", field.Name, field.FieldType));
                    }

                    members.Sort(); //sort all fields and properties as one, but exclude methods which will all be listed at the end

                    var methodMembers = new List<string>();
                    var methods = targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var method in methods)
                    {
                        if (!method.Name.StartsWith("get_", StringComparison.Ordinal) && !method.Name.StartsWith("set_", StringComparison.Ordinal))
                        {
                            methodMembers.Add(method.ToString());
                        }
                    }

                    methodMembers.Sort();
                    members.AddRange(methodMembers);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(CultureInfo.InvariantCulture, "Error loading members for the target Type: \r\n\n{0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return members;
        }

        #endregion

        #region RuleSet actions

        private void copyButton_Click(object sender, EventArgs e)
        {
            if (selectedRuleSetData != null)
            {
                var newData = selectedRuleSetData.Clone();
                int newMajor;
                int newMinor;

                GenerateNewVersionInfo(selectedRuleSetData, out newMajor, out newMinor);
                newData.MajorVersion = newMajor;
                newData.MinorVersion = newMinor;

                MarkDirty(newData);
                AddRuleSetData(newData);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (selectedRuleSetData != null)
            {
                if (selectedRuleSetData.Activity != null)
                {
                    var ruleSetDialog = new RuleSetDialog(selectedRuleSetData.Activity, null, selectedRuleSetData.RuleSet);
                    MakeRuleSetDialogResizable(ruleSetDialog);
                    var result = ruleSetDialog.ShowDialog();

                    if (result == DialogResult.OK) //check if they cancelled
                    {
                        selectedRuleSetData.RuleSet = ruleSetDialog.RuleSet;
                        MarkDirty(selectedRuleSetData);
                    }

                }
                else
                {
                    MessageBox.Show("You must first specify a target Activity for the RuleSet.", "RuleSet Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("You must first select a RuleSet.", "RuleSet Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private static void MakeRuleSetDialogResizable(RuleSetDialog ruleSetDialog)
        {
            ruleSetDialog.FormBorderStyle = FormBorderStyle.Sizable;
            ruleSetDialog.HelpButton = false;
            ruleSetDialog.MaximizeBox = true;
            ruleSetDialog.MinimumSize = new Size(580, 440);
            ruleSetDialog.Controls["okCancelTableLayoutPanel"].Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            ruleSetDialog.Controls["rulesGroupBox"].Controls["panel1"].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            ruleSetDialog.Controls["rulesGroupBox"].Controls["panel1"].Controls["chainingBehaviourComboBox"].Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ruleSetDialog.Controls["rulesGroupBox"].Controls["panel1"].Controls["chainingLabel"].Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ruleSetDialog.Controls["rulesGroupBox"].Controls["panel1"].Controls["rulesListView"].Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            ruleSetDialog.Resize += RuleSetDialog_Resize;
        }

        private static void RuleSetDialog_Resize(object sender, EventArgs e)
        {
            var ruleSetDialog = sender as Control;
            if (ruleSetDialog != null)
            {
                LayoutRuleSetDialog(ruleSetDialog);
            }
        }

        private static void LayoutRuleSetDialog(Control ruleSetDialog)
        {
            const int TopOffset = 50;
            const int BottomOffset = 50;
            const int Padding = 20;
            const int LeftMargin = 10;

            var availableHeight = ruleSetDialog.Size.Height - TopOffset - BottomOffset;
            var rulesGroupBoxHeight = availableHeight / 3;
            var ruleGroupBoxHeight = availableHeight - rulesGroupBoxHeight;
            var groupBoxWidth = ruleSetDialog.Size.Width - 20;

            var rulesGroupBox = ruleSetDialog.Controls["rulesGroupBox"];
            if (rulesGroupBox != null)
            {
                rulesGroupBox.SetBounds(LeftMargin, TopOffset, groupBoxWidth, rulesGroupBoxHeight - Padding);
            }

            var ruleGroupBox = ruleSetDialog.Controls["ruleGroupBox"];
            if (ruleGroupBox != null)
            {
                ruleGroupBox.SetBounds(LeftMargin, rulesGroupBoxHeight + TopOffset, groupBoxWidth, ruleGroupBoxHeight - Padding);
                LayoutRuleGroupBox(ruleGroupBox);
            }
        }

        private static void LayoutRuleGroupBox(Control ruleGroupBox)
        {
            const int TopOffset = 80;
            const int Padding = 20;
            const int LeftMargin = 10;
            const int LabelHeight = 17;

            var textBoxHeight = (ruleGroupBox.Size.Height - TopOffset) / 3;
            var textBoxWidth = ruleGroupBox.Size.Width - 20;

            var conditionTextBox = ruleGroupBox.Controls["conditionTextBox"];
            if (conditionTextBox != null)
            {
                conditionTextBox.SetBounds(LeftMargin, TopOffset, textBoxWidth, textBoxHeight - Padding);
            }

            var conditionLabel = ruleGroupBox.Controls["conditionLabel"];
            if (conditionLabel != null)
            {
                conditionLabel.SetBounds(LeftMargin, TopOffset - LabelHeight, textBoxWidth, LabelHeight);
            }

            var thenTextBox = ruleGroupBox.Controls["thenTextBox"];
            if (thenTextBox != null)
            {
                thenTextBox.SetBounds(LeftMargin, textBoxHeight + TopOffset, textBoxWidth, textBoxHeight - Padding);
            }

            var thenLabel = ruleGroupBox.Controls["thenLabel"];
            if (thenLabel != null)
            {
                thenLabel.SetBounds(LeftMargin, textBoxHeight + TopOffset - LabelHeight, textBoxWidth, LabelHeight);
            }

            var elseTextBox = ruleGroupBox.Controls["elseTextBox"];
            if (elseTextBox != null)
            {
                elseTextBox.SetBounds(LeftMargin, (textBoxHeight * 2) + TopOffset, textBoxWidth, textBoxHeight - Padding);
            }

            var elseLabel = ruleGroupBox.Controls["elseLabel"];
            if (elseLabel != null)
            {
                elseLabel.SetBounds(LeftMargin, (textBoxHeight * 2) + TopOffset - LabelHeight, textBoxWidth, LabelHeight);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var selectedNode = treeView1.SelectedNode;
            var parentNode = selectedNode.Parent;

            if (IsVersionNode(selectedNode) && selectedRuleSetData != null)
            {
                deletedRuleSetDataCollection.Add(selectedRuleSetData);
                MarkDirty(selectedRuleSetData);

                ruleSetDataDictionary.Remove(selectedNode);
                parentNode.Nodes.Remove(selectedNode);

                //if this was the only version node, remove the ruleset name node
                if (parentNode.Nodes.Count == 0)
                {
                    treeView1.Nodes.Remove(parentNode);
                }

                //selectedRuleSetData = null;
                //assemblyPath = null;
                SetSelectedNode(null);
            }
        }

        private TreeNode GetTreeNodeForRuleSetData(RuleSetData data)
        {
            if (data != null)
            {
                var enumerator = ruleSetDataDictionary.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var otherData = enumerator.Current.Value;
                    if (String.CompareOrdinal(otherData.Name, data.Name) == 0 && otherData.MajorVersion == data.MajorVersion && otherData.MinorVersion == data.MinorVersion)
                        return enumerator.Current.Key;
                }
            }
            return null;
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            var newData = CreateRuleSetData(null);
            AddRuleSetData(newData);
        }

        private RuleSetData CreateRuleSetData(System.Workflow.Activities.Rules.RuleSet ruleSet)
        {
            var data = new RuleSetData();
            if (ruleSet != null)
            {
                data.Name = ruleSet.Name;
                data.RuleSet = ruleSet;
            }
            else
            {
                data.Name = GenerateRuleSetName();
                data.RuleSet = new System.Workflow.Activities.Rules.RuleSet(data.Name);
            }
            data.MajorVersion = 1;
            MarkDirty(data);
            return data;
        }

        #endregion

        #region Event handlers

        // this is needed since the Validating events aren't fired for the fields if a menu item is selected
        // note that after this event fires, the user can still select a menu item, but the field values will have been reset (in this method)
        // could add similar logic for each tool strip item Click, but this is sufficient
        void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (selectedRuleSetData != null)
            {
                var cancelArgs = new CancelEventArgs(false);
                ruleSetNameBox_Validating(this, cancelArgs);
                if (cancelArgs.Cancel)
                {
                    ruleSetNameBox.Text = selectedRuleSetData.Name; // name is invalid so set it back (since they can still navigate the menu)
                }

                cancelArgs.Cancel = false;
                majorVersionBox_Validating(this, cancelArgs);
                if (cancelArgs.Cancel)
                {
                    majorVersionBox.Value = selectedRuleSetData.MajorVersion;
                }

                cancelArgs.Cancel = false;
                minorVersionBox_Validating(this, cancelArgs);
                if (cancelArgs.Cancel)
                {
                    minorVersionBox.Value = selectedRuleSetData.MinorVersion;
                }
            }
        }

        void ruleSetNameBox_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            if (selectedRuleSetData != null)
            {
                if (String.IsNullOrEmpty(ruleSetNameBox.Text))
                {
                    MessageBox.Show("RuleSet Name cannot be empty.", "RuleSet Property Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ruleSetNameBox.Text = selectedRuleSetData.Name;
                }
                else if (ruleSetNameBox.Text != selectedRuleSetData.Name)
                {
                    RuleSetData duplicateData;
                    if (!IsDuplicateRuleSet(ruleSetNameBox.Text, selectedRuleSetData.MajorVersion, selectedRuleSetData.MinorVersion, out duplicateData)
                        || duplicateData == selectedRuleSetData)
                    {
                        selectedRuleSetData.Name = ruleSetNameBox.Text;
                        MarkDirty(selectedRuleSetData);

                        var ruleSetDataCollection = new List<RuleSetData>();
                        foreach (var data in ruleSetDataDictionary.Values)
                            ruleSetDataCollection.Add(data);

                        BuildTree(ruleSetDataCollection);
                        SetSelectedNode(GetTreeNodeForRuleSetData(selectedRuleSetData));
                    }
                    else
                    {
                        MessageBox.Show("A RuleSet with the same name and version numbers already exists.", "RuleSet Property Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
                }
            }
        }

        void majorVersionBox_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            var majorInt = Convert.ToInt32(majorVersionBox.Value);
            if (selectedRuleSetData != null && treeView1.SelectedNode != null && majorInt != selectedRuleSetData.MajorVersion)
            {
                if (majorInt > 0)
                {
                    RuleSetData duplicateData;
                    if (!IsDuplicateRuleSet(selectedRuleSetData.Name, Convert.ToInt32(majorVersionBox.Value), selectedRuleSetData.MinorVersion, out duplicateData)
                        || duplicateData == selectedRuleSetData)
                    {
                        selectedRuleSetData.MajorVersion = majorInt;
                        MarkDirty(selectedRuleSetData);

                        var selectedNode = treeView1.SelectedNode;
                        selectedNode.Text = VersionTreeNodeText(selectedRuleSetData.MajorVersion, selectedRuleSetData.MinorVersion);
                        treeView1.Sort();
                        SetSelectedNode(selectedNode);
                    }
                    else
                    {
                        MessageBox.Show("A RuleSet with the same name and version numbers already exists.", "RuleSet Property Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
                }
                else
                {
                    MessageBox.Show("Major version number must be greater than 0", "RuleSet Property Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                }
            }
        }

        void minorVersionBox_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            var minorInt = Convert.ToInt32(minorVersionBox.Value);
            if (selectedRuleSetData != null && treeView1.SelectedNode != null && minorInt != selectedRuleSetData.MinorVersion)
            {
                RuleSetData duplicateData;
                if (!IsDuplicateRuleSet(selectedRuleSetData.Name, selectedRuleSetData.MajorVersion, Convert.ToInt32(minorVersionBox.Value), out duplicateData)
                    || duplicateData == selectedRuleSetData)
                {
                    selectedRuleSetData.MinorVersion = minorInt;
                    MarkDirty(selectedRuleSetData);

                    var selectedNode = treeView1.SelectedNode;
                    selectedNode.Text = VersionTreeNodeText(selectedRuleSetData.MajorVersion, selectedRuleSetData.MinorVersion);
                    treeView1.Sort();
                    SetSelectedNode(selectedNode);
                }
                else
                {
                    MessageBox.Show("A RuleSet with the same name and version numbers already exists.", "RuleSet Property Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
        }

        private static string VersionTreeNodeText(int majorVersion, int minorVersion)
        {
            return String.Format(CultureInfo.InvariantCulture, "Version {0}.{1}", majorVersion, minorVersion);
        }

        #endregion

        #region Other

        private static bool IsVersionNode(TreeNode node)
        {
            if (node != null)
                return node.Text.StartsWith("Version", StringComparison.Ordinal);
            return false;
        }

        private void AddRuleSetData(RuleSetData ruleSetData)
        {
            if (ruleSetData != null)
            {
                var parentNode = FindParentNode(ruleSetData);

                if (parentNode == null)
                {
                    parentNode = new TreeNode(ruleSetData.Name);
                    treeView1.Nodes.Add(parentNode);
                }

                var newVersionNode = new TreeNode(VersionTreeNodeText(ruleSetData.MajorVersion, ruleSetData.MinorVersion));
                parentNode.Nodes.Add(newVersionNode);
                treeView1.Sort();
                ruleSetDataDictionary.Add(newVersionNode, ruleSetData);
                SetSelectedNode(newVersionNode);
            }
        }

        private void MarkDirty(RuleSetData data)
        {
            if (data != null)
                data.Dirty = true;

            dirty = true;
        }

        private bool IsDuplicateRuleSet(string name, int majorVersion, int minorVersion, out RuleSetData duplicateRuleSetData)
        {
            foreach (var data in ruleSetDataDictionary.Values)
            {
                if (String.CompareOrdinal(data.Name, name) == 0 && data.MajorVersion == majorVersion && data.MinorVersion == minorVersion)
                {
                    duplicateRuleSetData = data;
                    return true;
                }
            }
            duplicateRuleSetData = null;
            return false;
        }

        private string GenerateRuleSetName()
        {
            var namePrefix = "RuleSet";
            var newName = "";
            var uniqueNameNotFound = true;
            var counter = 0;

            while (uniqueNameNotFound)
            {
                counter++;
                uniqueNameNotFound = false;
                newName = namePrefix + counter.ToString(CultureInfo.InvariantCulture);
                uniqueNameNotFound = IsDuplicateRuleSetName(newName);
            }

            return newName;
        }

        private void GenerateNewVersionInfo(RuleSetData currentRuleSetData, out int newMajorVersion, out int newMinorVersion)
        {
            var rsdOfInterest = new List<RuleSetData>();
            foreach (var data in ruleSetDataDictionary.Values)
            {
                if (data.Name == currentRuleSetData.Name && ((data.MajorVersion > currentRuleSetData.MajorVersion) || (data.MajorVersion == currentRuleSetData.MajorVersion && data.MinorVersion > currentRuleSetData.MinorVersion)))
                    rsdOfInterest.Add(data);
            }
            rsdOfInterest.Sort();

            var nextMajorTaken = false;
            var lastMajorUsed = currentRuleSetData.MajorVersion;
            var lastMinorUsed = currentRuleSetData.MinorVersion;
            foreach (var data in rsdOfInterest)
            {
                lastMajorUsed = data.MajorVersion;

                if (data.MajorVersion == currentRuleSetData.MajorVersion)
                    lastMinorUsed = data.MinorVersion;

                if (data.MajorVersion == currentRuleSetData.MajorVersion + 1)
                    nextMajorTaken = true;
            }

            if (!nextMajorTaken)
            {
                newMajorVersion = currentRuleSetData.MajorVersion + 1;
                newMinorVersion = 0;
            }
            else if (lastMinorUsed < maxMinorVersions - 1)
            {
                newMajorVersion = currentRuleSetData.MajorVersion;
                newMinorVersion = lastMinorUsed + 1;
            }
            else if (lastMajorUsed < maxMajorVersions - 1)
            {
                newMajorVersion = lastMajorUsed + 1;
                newMinorVersion = 0;
            }
            else
            {
                newMajorVersion = currentRuleSetData.MajorVersion;
                newMinorVersion = currentRuleSetData.MinorVersion;
                MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "Only {0} major versions are allowed for a single RuleSet name.  \r\nYou must manually change the version information.", maxMajorVersions), "RuleSet Property Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetNextMajorVersion(string ruleSetName)
        {
            var highestMajorVersionNumber = 0;

            foreach (var data in ruleSetDataDictionary.Values)
            {
                if (String.CompareOrdinal(data.Name, ruleSetName) == 0 && data.MajorVersion > highestMajorVersionNumber)
                    highestMajorVersionNumber = data.MajorVersion;
            }

            return highestMajorVersionNumber + 1;
        }

        private bool IsDuplicateRuleSetName(string name)
        {
            foreach (var data in ruleSetDataDictionary.Values)
            {
                if (String.CompareOrdinal(data.Name, name) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool GetThisType(string rulesFileDirectoryPath, string rulesFileName)
        {
            var successfulLoad = false;

            if (!string.IsNullOrEmpty(rulesFileDirectoryPath) && selectedRuleSetData != null)
            {
                var thisTypeAssemblyPath = rulesFileDirectoryPath + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Debug";
                if (Directory.Exists(thisTypeAssemblyPath))
                {
                    assemblyPath = thisTypeAssemblyPath;
                    var fileNames = Directory.GetFiles(thisTypeAssemblyPath);
                    var candidateThisTypes = new Dictionary<Type, string>();

                    //try and automatically locate the Type referenced by this ruleset
                    foreach (var fileName in fileNames)
                    {
                        if (fileName.EndsWith("dll", StringComparison.Ordinal) || fileName.EndsWith("exe", StringComparison.Ordinal))
                        {
                            Assembly assembly = null;
                            try
                            {
                                assembly = Assembly.LoadFile(fileName); //this will skip the load if it's already been loaded, which is a problem if you point to a different assembly with the same version number
                            }
                            catch (Exception) //ignore this assembly then
                            {
                            }

                            if (assembly != null)
                            {
                                foreach (var type in assembly.GetTypes())
                                {
                                    try
                                    {
                                        var validation = new RuleValidation(type, null);
                                        if (selectedRuleSetData.RuleSet.Validate(validation))
                                            candidateThisTypes.Add(type, fileName); // type matches the ruleset members
                                    }
                                    catch (Exception) //error creating RuleValidation or doing validation so ignore this type
                                    {
                                    }
                                }
                            }
                        }
                    }

                    if (candidateThisTypes.Count == 0) //no matching types found so prompt the user
                    {
                        successfulLoad = PromptForThisType(rulesFileDirectoryPath);
                    }
                    else if (candidateThisTypes.Count == 1) //one matching Type in the assemblies in the default path, so just use it
                    {
                        IEnumerator enumerator = candidateThisTypes.Keys.GetEnumerator();
                        enumerator.MoveNext();
                        selectedRuleSetData.Activity = enumerator.Current as Type;
                        selectedRuleSetData.AssemblyPath = candidateThisTypes[selectedRuleSetData.Activity];
                        successfulLoad = true;
                    }
                    else //more than one matching Type
                    {
                        //see if there is a single Type with the same name as the .rules file
                        var candidateThisTypesMatchingName = new Dictionary<Type, string>();
                        foreach (var type in candidateThisTypes.Keys)
                        {
                            if (type.Name == Path.GetFileNameWithoutExtension(rulesFileName))
                                candidateThisTypesMatchingName.Add(type, candidateThisTypes[type]);
                        }
                        if (candidateThisTypesMatchingName.Count == 1)
                        {
                            IEnumerator enumerator = candidateThisTypesMatchingName.Keys.GetEnumerator();
                            enumerator.MoveNext();
                            selectedRuleSetData.Activity = enumerator.Current as Type;
                            selectedRuleSetData.AssemblyPath = candidateThisTypesMatchingName[selectedRuleSetData.Activity];
                            successfulLoad = true;
                        }
                        else
                        {
                            successfulLoad = PromptForThisType(thisTypeAssemblyPath);
                        }
                    }
                }
                else
                {
                    successfulLoad = PromptForThisType(rulesFileDirectoryPath);
                }
            }
            return successfulLoad;
        }

        private bool PromptForThisType(string startingDirectory)
        {
            var successfulLoad = false;

            if (selectedRuleSetData != null)
            {
                var activitySelector = new ActivitySelector();
                activitySelector.RuleSet = selectedRuleSetData.RuleSet;
                if (!String.IsNullOrEmpty(startingDirectory))
                    activitySelector.InitialDirectory = startingDirectory;

                activitySelector.ShowDialog();
                if (!string.IsNullOrEmpty(activitySelector.AssemblyPath))
                    selectedRuleSetData.AssemblyPath = activitySelector.AssemblyPath;
                if (activitySelector.Activity != null)
                {
                    selectedRuleSetData.Activity = activitySelector.Activity;
                    successfulLoad = true;
                }
            }

            return successfulLoad;
        }

        #endregion

    }

    internal class TreeSortClass : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            var versionNodePrefix = "Version";
            var xNode = x as TreeNode;
            var yNode = y as TreeNode;

            if (xNode.Text.StartsWith(versionNodePrefix, StringComparison.Ordinal))
            {
                var xVersionString = xNode.Text.Substring(versionNodePrefix.Length);
                var yVersionString = yNode.Text.Substring(versionNodePrefix.Length);

                var xMajor = Int32.Parse(xVersionString.Substring(0, xVersionString.IndexOf(".", StringComparison.Ordinal)), CultureInfo.InvariantCulture);
                var xMinor = Int32.Parse(xVersionString.Substring(xVersionString.IndexOf(".", StringComparison.Ordinal) + 1), CultureInfo.InvariantCulture);
                var yMajor = Int32.Parse(yVersionString.Substring(0, yVersionString.IndexOf(".", StringComparison.Ordinal)), CultureInfo.InvariantCulture);
                var yMinor = Int32.Parse(yVersionString.Substring(yVersionString.IndexOf(".", StringComparison.Ordinal) + 1), CultureInfo.InvariantCulture);

                if (xMajor != yMajor)
                {
                    return xMajor - yMajor;
                }
                if (xMinor != yMinor)
                {
                    return xMinor - yMinor;
                }
                MessageBox.Show("Two RuleSets exist with the same name and version numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            return String.CompareOrdinal(xNode.Text, yNode.Text);
        }
    }
}