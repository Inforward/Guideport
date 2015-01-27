
(function (portal, $, _) {

    portal.businessPlanningWizard = function () {
        var viewModelObservable,
            trim = $.trim,
            errorWindow,
            $wizard,
            $element,
            $autoSaveMessages,
            $tabs,
            $roleDialog,
            $objectiveDialog,
            $autoSaveLabel,

            config = {
                baseUrl: '/pentameter/business-management/strategic-planning/business-planning',
                selectedYear: null,
                maximumObjectives: 5
            },
            
            dataTypes = {
                String: 'string',
                Integer: 'integer',
                Decimal: 'decimal',
                Percent: 'percent'
            };

        var viewModel = {

            //
            // These properties will be initially set from data retrieved from the server
            //
            BusinessPlan: null,
            Objectives: [],
            EmployeeRoles: [],
            AvailablePlanYears: [],
            ExistingPlanYears: [],

            //
            // Create Plan Vars
            //
            selectedYear: null,
            selectedNewYear: null,
            selectedCopyFromYear: null,

            //
            // Misc. Vars
            //
            swotColorEnabled: true,
            isDirty: false,
            isSaving: null,
            selectedTabIndex: 0,
            totalTabs: 5,
            creatingNewPlan: false,

            //
            // Methods
            //
            saveBusinessPlan: function(callback) {
                var that = this;

                if (!that.saveEnabled()) {
                    if (typeof (callback) == "function")
                        callback.call(this);
                    return;
                }

                that.set("isSaving", true);

                portal.dataManager.send({
                    url: config.baseUrl + "/JsonUpdateBusinessPlan",
                    data: kendo.stringify(this.get("BusinessPlan")),
                    success: function(response) {
                        that.set("isDirty", false);
                        if (typeof (callback) == "function")
                            callback.call(that, response);
                    }
                }).always(function() {
                    that.set("isSaving", false);
                });
            },

            saveClicked: function() {
                this.saveBusinessPlan();
            },

            saveEnabled: function() {
                return this.get("isDirty") && !(this.get("isSaving") || false);
            },

            completeClicked: function() {
                this.saveBusinessPlan(function() {
                    var year = this.get("BusinessPlan.Year");

                    setTimeout(function() {
                        window.location = kendo.format("{0}/my-business-plans?year={1}", config.baseUrl, year);
                    }, 500);
                });
            },

            deleteBusinessPlan: function() {
                var year = this.get("BusinessPlan.Year"),
                    that = this;

                if (!confirm('Are you sure you want to delete your ' + year + ' Business Plan?'))
                    return;

                portal.dataManager.send({
                    url: config.baseUrl + "/JsonDeleteBusinessPlan",
                    data: kendo.stringify({ businessPlanId: this.get("BusinessPlan.BusinessPlanID") }),
                    success: function(data) {
                        that.resetData(data);
                    }
                });
            },

            createBusinessPlan: function() {
                var selectedYear = this.get("selectedNewYear"),
                    copyFromYear = this.get("selectedCopyFromYear") || "",
                    that = this;

                if (that.get("creatingNewPlan") === true)
                    return;

                if (selectedYear == null || selectedYear.length == 0) {
                    showErrorModal("Please select a year to create a new plan for");
                    return;
                }

                that.set("creatingNewPlan", true);

                portal.dataManager.send({
                    url: config.baseUrl + "/JsonCreateBusinessPlan/" + selectedYear + "/" + copyFromYear,
                    success: function(data) {

                        that.resetData(data);
                        $("#editor-new-plan-dialog").modal('hide');
                    }
                }).always(function() {
                    that.set("creatingNewPlan", false);
                });

            },

            gotoBusinessPlan: function(e) {
                var $target = $(e.currentTarget),
                    year = $target.data("year");

                if (!this.checkForChanges())
                    return;

                if (year != null) {
                    this.set("selectedYear", year);
                    this.populate();
                }
            },

            getExportUrl: function() {
                return kendo.format("{0}pdf/ExecuteUrl?url={1}/export/{2}&type=BusinessPlan&title={2}%20Business%20Plan",
                    portal.rootUrl,
                    config.baseUrl,
                    this.get("BusinessPlan.Year"));
            },

            exportBusinessPlan: function() {
                if (this.get("isDirty") === true) {
                    this.saveBusinessPlan();
                }
            },

            checkForChanges: function(e) {
                var ok = true;

                if (this.get("isDirty") === true)
                    ok = confirm("You have usaved changes to this Business Plan.  Are you sure you want to proceed?");

                if (!ok && e)
                    e.preventDefault();

                return ok;
            },

            populate: function(callback) {
                var that = this,
                    year = that.get("selectedYear");

                $wizard.fadeOut(function() {
                    portal.dataManager.send({
                        url: config.baseUrl + "/JsonGetBusinessPlan/" + year,
                        success: function(data) {
                            that.resetData(data);

                            if (callback)
                                callback.call(that, data);

                            $wizard.fadeIn();
                        }
                    });
                });
            },

            refreshDataSources: function() {
                this.get("swots").read();
                this.get("roles").read();
                this.get("employees").read();
                this.get("objectives").read();
                this.get("strategies").read();
                this.get("tactics").read();
            },

            resetData: function(data) {

                for (var prop in data) {
                    this.set(prop, data[prop]);
                }

                this.refreshDataSources();

                this.set("isDirty", false);
                this.set("selectedNewYear", null);
                this.set("selectedCopyFromYear", null);
                this.set("selectedTabIndex", 0);
                this.set("rolesVisible", true);
                this.selectTab();
            },


            //
            // Navigation Methods
            //
            tabSelected: function(e) {
                this.set("selectedTabIndex", $(e.item).index());
            },

            nextTab: function() {
                var currentIndex = this.get("selectedTabIndex");

                if (currentIndex < (this.totalTabs - 1))
                    this.set("selectedTabIndex", ++currentIndex);

                this.selectTab();
            },

            previousTab: function() {
                var currentIndex = this.get("selectedTabIndex");

                if (currentIndex > 0)
                    this.set("selectedTabIndex", --currentIndex);

                this.selectTab();
            },

            selectTab: function() {
                var tabStrip = $tabs.data("kendoTabStrip");

                if (tabStrip)
                    tabStrip.select(this.get("selectedTabIndex"));
            },

            isTabSelected: function(tabIndex) {
                return this.get("selectedTabIndex") === tabIndex;
            },


            //
            // Objective Methods
            //
            currentObjectiveStep: 1,
            currentObjective: null,
            selectedObjective: null,

            objectives: new kendo.data.DataSource({
                transport: {
                    read: { url: config.baseUrl + '/JsonGetObjectives' },
                    create: { url: config.baseUrl + '/JsonCreateObjective' },
                    update: { url: config.baseUrl + '/JsonUpdateObjective' },
                    destroy: { url: config.baseUrl + '/JsonDeleteObjective' },
                    parameterMap: function(data, type) {
                        if (type == "read") {
                            return kendo.stringify({ businessPlanId: viewModelObservable.get("BusinessPlan.BusinessPlanID") });
                        } else {
                            return kendo.stringify({ objective: data });
                        }
                    }
                },
                sort: {
                    field: "Name",
                    dir: "asc"
                },
                error: function (e) {
                    if (e.errors) alert(e.errors);
                    this.cancelChanges();
                },
                schema: {
                    model: { id: "ObjectiveID" },
                    errors: function(response) {
                        if (response.Success !== true)
                            return response.Message;
                        return false;
                    },
                    data: "Data"
                }
            }),

            addObjective: function() {
                if (this.maxObjectivesReached()) {
                    showErrorModal("We're sorry.  You may only enter a maximum of " + config.maximumObjectives + " business goals.");
                    return;
                }

                var objective = {
                    Name: null,
                    Value: null,
                    EstimatedCompletionDate: new Date(this.get("BusinessPlan.Year"), 11, 31),
                    DataType: dataTypes.String,
                    Strategies: [],
                    AutoTrackingEnabled: false,
                    BusinessPlanID: this.get("BusinessPlan.BusinessPlanID")
                };

                this.set("currentObjective", objective);

                this.showObjectiveDialog();
            },

            editObjective: function(e) {
                e.preventDefault();

                var objective = {
                    Name: e.model.Name,
                    Value: e.model.Value,
                    EstimatedCompletionDate: new Date(e.model.EstimatedCompletionDate),
                    DataType: e.model.DataType,
                    ObjectiveID: e.model.ObjectiveID,
                    Strategies: [],
                    AutoTrackingEnabled: e.model.AutoTrackingEnabled,
                    BusinessPlanID: e.model.BusinessPlanID
                };

                $.each(e.model.Strategies, function(idx, strategy) {
                    objective.Strategies.push(strategy);
                });

                this.set("currentObjective", objective);

                this.setSelectedStrategies();

                this.showObjectiveDialog();
            },

            removeObjective: function(e) {
                if (!confirm("Are you sure you want to delete " + e.model.Name + "?")) {
                    e.preventDefault();
                    return;
                }
            },

            finishObjective: function() {
                var objectives = this.get("objectives"),
                    objective = this.get("currentObjective");

                this.cancelObjectiveListViewEdits();

                if (!this.isObjectiveValid())
                    return;

                // Add or Update
                if (this.isNewObjective()) {
                    objectives.add(objective);

                } else {
                    var existingObjective = objectives.get(objective.ObjectiveID);

                    existingObjective.set("Name", objective.Name);
                    existingObjective.set("Value", objective.Value);
                    existingObjective.set("EstimatedCompletionDate", objective.EstimatedCompletionDate);
                    existingObjective.set("Strategies", objective.Strategies);
                    existingObjective.set("dirty", true);
                }

                objectives.sync();

                this.closeObjectiveDialog();
            },

            showObjectiveDialog: function() {
                $objectiveDialog.modal('show');
            },

            closeObjectiveDialog: function() {
                $objectiveDialog.modal('hide');
                this.resetObjectiveDialog();
            },

            resetObjectiveDialog: function() {
                this.set("selectedObjective", null);
                this.set("currentObjectiveStep", 1);
                this.set("currentStrategyIndex", 0);
                this.resetAddStrategy();
                this.resetAddTactic();
                this.resetSelectedStrategies();
                this.resetSelectedTactics();
                this.cancelObjectiveListViewEdits();
            },

            cancelObjectiveListViewEdits: function() {
                $objectiveDialog.find("[data-role='listview']").each(function() {
                    $(this).data("kendoListView").cancel();
                });
            },

            getObjectiveStepOneText: function() {
                return this.isNewObjective() ? "Create an objective" : "Edit objective";
            },

            getObjectiveNameTooltip: function() {
                var autoTracked = this.get("currentObjective.AutoTrackingEnabled") === true;
                return autoTracked ? "The title of systematically tracked goals cannot be altered" : "";
            },

            selectedObjectiveChanged: function(e) {
                var value = e.sender.value(),
                    currentObjective = this.get("currentObjective");

                var objective = _.find(this.get("Objectives"), function(o) { return o.uid == value; });

                if (objective) {
                    currentObjective.set("Name", objective.Name);
                    currentObjective.set("DataType", objective.DataType);
                    currentObjective.set("AutoTrackingEnabled", objective.AutoTrackingEnabled);
                } else {
                    currentObjective.set("Name", value);
                    currentObjective.set("DataType", dataTypes.String);
                    currentObjective.set("AutoTrackingEnabled", false);
                }

                currentObjective.set("Value", null);
            },

            nextObjectiveStep: function() {
                var currentStep = this.get("currentObjectiveStep");

                if (currentStep < 3 && this.isObjectiveStepValid())
                    this.goToObjectiveStep(++currentStep);
            },

            previousObjectiveStep: function() {
                var currentStep = this.get("currentObjectiveStep");

                if (currentStep > 1)
                    this.goToObjectiveStep(--currentStep);
            },

            goToObjectiveStep: function(stepNo) {
                switch (stepNo) {
                case 2:
                    {
                        this.resetAddStrategy();
                    }
                    break;
                case 3:
                    {
                        this.set("currentStrategyIndex", 0);
                        this.setSelectedTactics();
                    }
                    break;
                }

                this.set("currentObjectiveStep", stepNo);
            },

            getObjectiveValueFormatted: function(objective) {
                var formattedValue = objective.Value,
                    dataType = objective.DataType;

                if (dataType == dataTypes.Integer) {
                    formattedValue = kendo.toString(parseInt(objective.Value), 'n0');
                } else if (dataType == dataTypes.Decimal) {
                    formattedValue = kendo.toString(parseFloat(objective.Value), 'c2');
                } else if (dataType == dataTypes.Percent) {
                    formattedValue = kendo.toString(parseFloat(objective.Value), 'n2') + ' %';
                }

                return formattedValue;
            },

            isObjectiveStepSelected: function(stepNo) {
                return this.get("currentObjectiveStep") === stepNo;
            },

            isObjectiveStepValid: function() {
                var currentStep = this.get("currentObjectiveStep"),
                    errors = [];

                switch (currentStep) {
                case 1:
                    {
                        errors = this.validateObjective();
                    }
                    break;
                case 2:
                    {
                        errors = this.validateObjectiveStrategies();
                    }
                    break;
                case 3:
                    {
                        errors = this.validateObjectiveStrategiesTactics();
                    }
                    break;
                }

                if (errors.length)
                    showErrorModal(errors);

                return errors.length == 0;
            },

            validateObjective: function() {
                var errors = [],
                    objective = this.get("currentObjective");

                if (objective == null)
                    return errors;

                if (trim(objective.Name).length <= 0)
                    errors.push("Please select or enter an objective title");

                if (trim(objective.Value).length <= 0)
                    errors.push("Please enter a goal");

                if (objective.EstimatedCompletionDate == null)
                    errors.push("Please enter an estimated completion date");

                var existing = this.findObjectiveByName(objective.Name);

                if (existing && (existing.ObjectiveID != objective.ObjectiveID))
                    errors = ["An objective with this name already exists.  Please enter a unique name."];

                return errors;
            },

            validateObjectiveStrategies: function() {
                var strategies = this.getObjectiveStrategies(),
                    errors = [];

                if (strategies.length == 0)
                    errors.push("Please select at least one strategy to support this objective");

                return errors;
            },

            validateObjectiveStrategiesTactics: function() {
                var errors = [],
                    emptyStrategies = _.filter(this.getObjectiveStrategies(), function(s) { return s.get("Tactics").length == 0; });

                if (emptyStrategies.length)
                    errors.push("Please ensure all strategies have at least one selected tactic");

                return errors;
            },

            isObjectiveValid: function() {
                return this.validateObjective().length == 0 &&
                    this.validateObjectiveStrategies().length == 0 &&
                    this.validateObjectiveStrategiesTactics().length == 0;
            },

            findObjectiveByName: function(name) {
                if (name == null)
                    return null;

                return _.find(this.get("objectives").data(), function(o) { return trim(o.Name.toLowerCase()) == trim(name.toLowerCase()); });
            },

            isNewObjective: function() {
                var id = this.get("currentObjective.ObjectiveID");
                return id == null || id <= 0;
            },

            isTextObjective: function() {
                return this.get("currentObjective.DataType") == dataTypes.String;
            },

            isIntegerObjective: function() {
                return this.get("currentObjective.DataType") == dataTypes.Integer;
            },

            isDecimalObjective: function() {
                return this.get("currentObjective.DataType") == dataTypes.Decimal;
            },

            isPercentObjective: function() {
                return this.get("currentObjective.DataType") == dataTypes.Percent;
            },

            getMinObjectiveDate: function() {
                var year = this.get("BusinessPlan.Year"),
                    date = new Date(year, 0, 1);

                return date;
            },

            getMaxObjectiveDate: function() {
                var year = this.get("BusinessPlan.Year"),
                    date = new Date(year, 11, 31);

                return date;
            },

            maxObjectivesReached: function() {
                return this.get("objectives").data().length >= config.maximumObjectives;
            },

            hasObjectives: function() {
                return this.get("objectives").data().length > 0;
            },


            //
            // Strategy Methods
            //
            newStrategyName: null,

            strategies: new kendo.data.DataSource({
                transport: {
                    read: { url: config.baseUrl + '/JsonGetStrategies' },
                    create: { url: config.baseUrl + '/JsonCreateStrategy' },
                    update: { url: config.baseUrl + '/JsonUpdateStrategy' },
                    destroy: { url: config.baseUrl + '/JsonDeleteStrategy' },
                    parameterMap: function(data, type) {
                        if (type == "read") {
                            return kendo.stringify({ businessPlanId: viewModelObservable.get("BusinessPlan.BusinessPlanID") });
                        } else {
                            return kendo.stringify({ strategy: data });
                        }
                    }
                },
                sort: [
                    { field: "Editable", dir: "desc" },
                    { field: "Name", dir: "asc" }
                ],
                error: function (e) {
                    if (e.errors) alert(e.errors);
                    this.cancelChanges();
                },
                schema: {
                    model: { id: "StrategyID" },
                    errors: function(response) {
                        if (response.Success !== true)
                            return response.Message;
                        return false;
                    },
                    data: "Data"
                },
                requestEnd: function(e) {
                    if (e.type == "destroy" || e.type == "update") {
                        viewModelObservable.get("objectives").read();
                    }
                }
            }),

            addRemoveStrategyToObjective: function(e) {
                var strategy = e.data,
                    selected = strategy.get("Selected"),
                    strategies = this.getObjectiveStrategies();

                if (selected) {
                    strategies.push(strategy);
                } else {
                    this.removeFromCollection(strategies, strategy, "StrategyID");
                }
            },

            setSelectedStrategies: function() {
                var strategies = this.get("strategies");

                strategies.pushUpdate($.map(this.getObjectiveStrategies(), function(s) {
                    return {
                        StrategyID: s.StrategyID,
                        Selected: true
                    };
                }));

                this.resetStrategiesListView();
            },

            resetSelectedStrategies: function () {
                var strategies = this.get("strategies");

                strategies.pushUpdate($.map(strategies.data(), function (s) {
                    return {
                        StrategyID: s.StrategyID,
                        Selected: false
                    };
                }));

                this.resetStrategiesListView();
            },

            resetStrategiesListView: function() {
                $("#strategies").data("kendoListView").refresh();
            },

            addStrategy: function() {
                var strategies = this.get("strategies");

                var strategy = {
                    Name: trim(this.get("newStrategyName")),
                    BusinessPlanID: this.get("BusinessPlan.BusinessPlanID"),
                    Selected: false,
                    Editable: true,
                    Tactics: []
                };

                if (!this.validateStrategy(strategy))
                    return;

                if (this.exists(strategy, strategies.data(), "Name"))
                    return;

                strategies.add(strategy);
                strategies.sync();

                this.resetAddStrategy();
            },

            findStrategyByName: function(name) {
                if (name == null)
                    return null;

                return _.find(this.get("strategies").data(), function(s) { return trim(s.Name.toLowerCase()) == trim(name.toLowerCase()); });
            },

            validateStrategy: function(strategy) {
                if (trim(strategy.Name).length == 0) {
                    showErrorModal("Please enter a name for this new strategy");
                    return false;
                }

                return true;
            },

            saveStrategy: function (e) {
                if (!this.validateStrategy(e.model)) {
                    e.preventDefault();
                    return;
                }

                this.saveEditItem(e);
            },

            cancelEditStrategy: function() {
                this.resetStrategiesListView();
            },

            removeStrategy: function(e) {
                var strategy = e.model;

                if (!confirm("Are you sure you want to delete " + strategy.Name + "?")) {
                    e.preventDefault();
                    return;
                }

                this.removeFromCollection(this.getObjectiveStrategies(), strategy, "StrategyID");
            },

            addStrategyInFocus: false,

            getStrategyManageListCssClass: function() {
                return "editor-manage-list" + (this.get("addStrategyInFocus") ? " focused" : "");
            },

            addStrategyFocus: function() {
                this.set("addStrategyInFocus", true);
            },

            resetAddStrategy: function() {
                this.set("newStrategyName", null);
                this.set("addStrategyInFocus", false);
                Placeholders.enable();
            },

            //
            // Tactics Methods
            //
            currentStrategyIndex: 0,

            tactics: new kendo.data.DataSource({
                transport: {
                    read: { url: config.baseUrl + '/JsonGetTactics' },
                    create: { url: config.baseUrl + '/JsonCreateTactic' },
                    update: { url: config.baseUrl + '/JsonUpdateTactic' },
                    destroy: { url: config.baseUrl + '/JsonDeleteTactic' },
                    parameterMap: function(data, type) {
                        if (type == "read") {
                            return kendo.stringify({ businessPlanId: viewModelObservable.get("BusinessPlan.BusinessPlanID") });
                        } else {
                            return kendo.stringify({ tactic: data });
                        }
                    }
                },
                sort: [
                    { field: "Editable", dir: "desc" },
                    { field: "Name", dir: "asc" }
                ],
                error: function (e) {
                    if (e.errors) alert(e.errors);
                    this.cancelChanges();
                },
                schema: {
                    model: { id: "TacticID" },
                    errors: function(response) {
                        if (response.Success !== true)
                            return response.Message;
                        return false;
                    },
                    data: "Data"
                },
                requestEnd: function (e) {
                    if (e.type == "destroy" || e.type == "update") {
                        viewModelObservable.get("strategies").read();
                        viewModelObservable.get("objectives").read();
                    }
                }
            }),

            nextStrategy: function() {
                if (this.lastStrategySelected())
                    return;

                if (this.getCurrentStrategy().get("Tactics").length == 0) {
                    showErrorModal("Please select at least one tactic to support this strategy");
                    return;
                }

                var currentIndex = this.get("currentStrategyIndex");

                this.goToStrategy(++currentIndex);
            },

            previousStrategy: function() {
                if (this.firstStrategySelected())
                    return;

                var currentIndex = this.get("currentStrategyIndex");

                this.goToStrategy(--currentIndex);
            },

            goToStrategy: function(index) {
                this.set("currentStrategyIndex", index);
                this.resetSelectedTactics();
                this.setSelectedTactics();
            },

            lastStrategySelected: function() {
                return this.get("currentStrategyIndex") >= (this.getObjectiveStrategies().length - 1);
            },

            firstStrategySelected: function() {
                return this.get("currentStrategyIndex") == 0;
            },

            getObjectiveStrategies: function() {
                return this.get("currentObjective.Strategies") || [];
            },

            getCurrentStrategy: function() {
                var strategies = this.getObjectiveStrategies(),
                    index = this.get("currentStrategyIndex");

                return strategies.length > index ? strategies[index] : null;
            },

            getCurrentStrategyName: function() {
                var strategy = this.getCurrentStrategy();

                return (strategy != null ? strategy.Name : "");
            },

            getStrategyCountText: function() {
                var current = this.get("currentStrategyIndex") + 1,
                    total = this.getObjectiveStrategies().length;

                return kendo.format("({0} of {1})", current, total);
            },

            getCurrentStrategyTactics: function() {
                var strategy = this.getCurrentStrategy();

                return (strategy != null ? strategy.get("Tactics") : []);
            },

            addRemoveTacticToStrategy: function(e) {
                var tactic = e.data,
                    selected = tactic.get("Selected"),
                    tactics = this.getCurrentStrategyTactics();

                var $item = $(e.currentTarget),
                    listView = $("#tactics").data("kendoListView");

                if (selected) {
                    tactics.push(tactic);
                } else {
                    this.removeFromCollection(tactics, tactic, "TacticID");
                }

                //if (selected)
                //    listView.edit($item.parents("tr").first());
            },

            setSelectedTactics: function() {
                var tactics = this.get("tactics");

                tactics.pushUpdate($.map(this.getCurrentStrategyTactics(), function (t) {
                    return {
                        TacticID: t.TacticID,
                        Selected: true
                    };
                }));

                this.refreshTacticsListView();
            },

            resetSelectedTactics: function () {
                var tactics = this.get("tactics");

                tactics.pushUpdate($.map(tactics.data(), function(t) {
                    return {
                        TacticID: t.TacticID,
                        Selected: false
                    };
                }));

                this.refreshTacticsListView();
            },

            refreshTacticsListView: function() {
                $("#tactics").data("kendoListView").refresh();
            },

            newTactic: { Name: null, Description: null },

            addTactic: function() {
                var tactics = this.get("tactics");

                var tactic = {
                    Name: this.get("newTactic").Name,
                    Description: this.get("newTactic").Description,
                    Selected: false,
                    Editable: true,
                    BusinessPlanID: this.get("BusinessPlan.BusinessPlanID")
                };

                if (!this.validateTactic(tactic))
                    return;

                if (this.exists(tactic, tactics.data(), "Name"))
                    return;

                tactics.add(tactic);
                tactics.sync();

                this.resetAddTactic();
            },

            validateTactic: function(tactic) {
                var errors = [];

                if (trim(tactic.Name).length == 0)
                    errors.push("Please create a name for your new tactic");

                if (trim(tactic.Description).length == 0)
                    errors.push("Please describe your tactical plan");

                if (errors.length)
                    showErrorModal(errors);

                return (errors.length == 0);
            },

            saveTactic: function (e) {
                if (!this.validateTactic(e.model)) {
                    e.preventDefault();
                    return;
                }

                this.saveEditItem(e);
            },

            cancelEditTactic: function () {
                this.refreshTacticsListView();
            },

            removeTactic: function (e) {
                var tactic = e.model;

                if (!confirm("Are you sure you want to delete " + tactic.Name + "?")) {
                    e.preventDefault();
                    return;
                }

                this.removeFromCollection(this.getCurrentStrategyTactics(), tactic, "TacticID");
            },

            addTacticInFocus: false,

            getTacticManageListCssClass: function () {
                return "editor-manage-list" + (this.get("addTacticInFocus") ? " focused" : "");
            },

            addTacticFocus: function () {
                this.set("addTacticInFocus", true);
            },

            resetAddTactic: function () {
                this.set("newTactic", { Name: null, Description: null });
                this.set("addTacticInFocus", false);
                Placeholders.enable();
            },



            //
            // Swot Methods
            //
            swot: { Type: 'Strength', Description: null },

            swots: new kendo.data.DataSource({
                transport: {
                    read: { url: config.baseUrl + '/JsonGetSwots' },
                    create: { url: config.baseUrl + '/JsonCreateSwot' },
                    update: { url: config.baseUrl + '/JsonUpdateSwot' },
                    destroy: { url: config.baseUrl + '/JsonDeleteSwot' },
                    parameterMap: function (data, type) {
                        if (type == "read") {
                            return kendo.stringify({ businessPlanId: viewModelObservable.get("BusinessPlan.BusinessPlanID") });
                        } else {
                            return kendo.stringify({ swot: data });
                        }
                    }
                },
                error: function (e) {
                    if (e.errors) alert(e.errors);
                    this.cancelChanges();
                },
                change: function (e) {
                    if (e.action == "itemchange")
                        return;

                    viewModelObservable.triggerSwotChange();
                },
                schema: {
                    model: { id: "SwotID" },
                    errors: function (response) {
                        if (response.Success !== true)
                            return response.Message;
                        return false;
                    },
                    data: "Data"
                }
            }),

            triggerSwotChange: function () {
                this.trigger("change", { field: "swotStrengths" });
                this.trigger("change", { field: "swotWeaknesses" });
                this.trigger("change", { field: "swotOpportunities" });
                this.trigger("change", { field: "swotThreats" });
            },

            swotStrengths: function () {
                return this.swotByType('Strength');
            },

            swotWeaknesses: function () {
                return this.swotByType('Weakness');
            },

            swotOpportunities: function () {
                return this.swotByType('Opportunity');
            },

            swotThreats: function () {
                return this.swotByType('Threat');
            },

            swotByType: function (type) {
                return kendo.data.Query.process(this.get("swots").view(), {
                    filter: {
                        field: 'Type', operator: 'eq', value: type
                    },
                    sort: {
                        field: "Description", dir: "asc"
                    }
                }).data;
            },

            addSwot: function() {
                var swot = this.get("swot"),
                    swots = this.get("swots");

                if (!this.validateSwot(swot))
                    return;

                swots.add({
                    Type: swot.Type,
                    Description: trim(swot.Description),
                    BusinessPlanID: this.get("BusinessPlan.BusinessPlanID")
                });

                swots.sync();

                // Reset
                swot.set("Description", null);
            },

            validateSwot: function(swot) {
                if (trim(swot.Description).length == 0) {
                    showErrorModal("Please enter a description for this " + swot.Type);
                    return false;
                }

                return true;
            },

            saveSwot: function (e) {
                var swots = this.get("swots"),
                    swot = swots.get(e.model.SwotID);

                if (!this.validateSwot(e.model)) {
                    e.preventDefault();
                    return;
                }

                swot.set("Description", e.model.Description);
                swots.sync();
            },

            removeSwot: function (e) {
                if (!confirm("Are you sure you want to delete this " + e.model.Type + "?")) {
                    e.preventDefault();
                    return;
                }

                var swots = this.get("swots");

                swots.remove(e.model);
                swots.sync();
            },

            cancelEditSwot: function(e) {
                var swots = this.get("swots"),
                    swot = swots.get(e.model.SwotID),
                    cleanSwot = _.find(swots._pristineData, function (s) { return s.SwotID == e.model.SwotID; });

                swot.set("Description", cleanSwot.Description);
                swot.set("dirty", false);
            },

            getSwotContainerClass: function() {
                return "swot-sections" + (this.get("swotColorEnabled") ? " color" : "");
            },


            //
            // Role Methods
            //
            currentRole: null,
            currentRoleStep: 1,
            selectedRole: null,

            roles: new kendo.data.DataSource({
                transport: {
                    read:    { url: config.baseUrl + '/JsonGetEmployeeRoles' },
                    create:  { url: config.baseUrl + '/JsonCreateEmployeeRole' },
                    update:  { url: config.baseUrl + '/JsonUpdateEmployeeRole' },
                    destroy: { url: config.baseUrl + '/JsonDeleteEmployeeRole' },
                    parameterMap: function (data, type) {
                        if (type == "read") {
                            return kendo.stringify({ businessPlanId: viewModelObservable.get("BusinessPlan.BusinessPlanID") });
                        } else {
                            return kendo.stringify({ employeeRole: data });
                        }
                    }
                },
                sort: {
                    field: "Name", dir: "asc"
                },
                error: function (e) {
                    if (e.errors) alert(e.errors);
                    this.cancelChanges();
                },
                schema: {
                    model: { id: "EmployeeRoleID" },
                    errors: function(response) {
                        if (response.Success !== true)
                            return response.Message;
                        return false;
                    },
                    data: "Data"
                },
                requestEnd: function (e) {
                    if (e.type != "read") {
                        viewModelObservable.get("employees").read();
                    }
                }
            }), 

            addRole: function () {
                var role = {
                    Name: null,
                    Description: null,
                    Employees: [],
                    BusinessPlanID: this.get("BusinessPlan.BusinessPlanID")
                };

                this.set("currentRole", role);

                this.showRoleDialog();
            },

            editRole: function (e) {
                e.preventDefault();

                var role = {
                    Name: e.model.Name,
                    Description: e.model.Description,
                    EmployeeRoleID: e.model.EmployeeRoleID,
                    Employees: []
                };

                $.each(e.model.Employees, function (idx, employee) {
                    role.Employees.push(employee);
                });

                this.set("currentRole", role);

                this.setSelectedEmployees();

                this.showRoleDialog();
            },

            removeRole: function (e) {
                if (!confirm("Are you sure you want to delete " + e.model.Name + "?")) {
                    e.preventDefault();
                    return;
                }
            },

            finishRole: function () {
                var roles = this.get("roles"),
                    role = this.get("currentRole");

                this.cancelRoleListViewEdits();

                if (!this.isRoleValid())
                    return;

                if (this.isNewRole()) {
                    roles.add(role);

                } else {
                    var existingRole = roles.get(role.EmployeeRoleID);

                    existingRole.set("Name", role.Name);
                    existingRole.set("Description", role.Description);
                    existingRole.set("Employees", role.Employees);
                    existingRole.set("dirty", true);
                }

                roles.sync();

                this.closeRoleDialog();
            },

            showRoleDialog: function () {
                $roleDialog.modal('show');
            },

            closeRoleDialog: function () {
                $roleDialog.modal('hide');
                this.resetRoleDialog();
            },

            resetRoleDialog: function () {
                this.set("selectedRole", null);
                this.set("currentRoleStep", 1);
                this.resetAddEmployee();
                this.resetSelectedEmployees();
                this.cancelRoleListViewEdits();
            },

            cancelRoleListViewEdits: function() {
                $roleDialog.find("[data-role='listview']").each(function () {
                    $(this).data("kendoListView").cancel();
                });
            },

            getRoleStepOneText: function () {
                return this.isNewRole() ? "Create a role" : "Edit role";
            },

            selectedRoleChanged: function (e) {
                var value = e.sender.value(),
                    currentRole = this.get("currentRole");

                var role = _.find(this.get("EmployeeRoles"), function(o) { return o.uid == value; }),
                    name = role != null ? role.Name : value;

                currentRole.set("Name", name);
            },

            nextRoleStep: function () {
                var currentStep = this.get("currentRoleStep");

                if (currentStep < 2 && this.isRoleStepValid())
                    this.goToRoleStep(++currentStep);
            },

            previousRoleStep: function () {
                var currentStep = this.get("currentRoleStep");

                if (currentStep > 1)
                    this.goToRoleStep(--currentStep);
            },

            goToRoleStep: function (stepNo) {
                this.set("currentRoleStep", stepNo);

                if (stepNo == 2)
                    this.resetAddEmployee();
            },

            isRoleStepSelected: function (stepNo) {
                return this.get("currentRoleStep") === stepNo;
            },

            isRoleStepValid: function () {
                var currentStep = this.get("currentRoleStep"),
                    errors = [];

                switch (currentStep) {
                    case 1: {
                            errors = this.validateRole();
                        }
                        break;
                    case 2: {
                            errors = this.validateRoleEmployees();
                        }
                        break;
                }

                if (errors.length)
                    showErrorModal(errors);

                return errors.length == 0;
            },

            validateRole: function () {
                var errors = [],
                    role = this.get("currentRole");

                if (role == null)
                    return errors;

                if (trim(role.Name).length == 0)
                    errors.push("Please select or enter an role");

                if (trim(role.Description).length == 0)
                    errors.push("Please enter a brief description for this role");

                var existing = this.findRoleByName(role.Name);

                if (existing && (existing.EmployeeRoleID != role.EmployeeRoleID))
                    errors = ["A role with this name already exists.  Please enter a unique name."];

                return errors;
            },

            validateRoleEmployees: function () {
                var employees = this.getRoleEmployees(),
                    errors = [];

                if (employees.length == 0)
                    errors.push("Please enter or select at least one employee for this role");

                return errors;
            },

            isRoleValid: function () {
                return this.validateRole().length == 0 &&
                       this.validateRoleEmployees().length == 0;
            },

            findRoleByName: function (name) {
                if (name == null)
                    return null;

                return _.find(this.get("roles").data(), function (o) { return trim(o.Name.toLowerCase()) == trim(name.toLowerCase()); });
            },

            isNewRole: function () {
                var id = this.get("currentRole.EmployeeRoleID");
                return id == null || id <= 0;
            },

            hasRoles: function() {
                return this.get("roles").data().length > 0;
            },

            rolesVisible: true,

            getRoleNavigationClass: function() {
                return this.get("rolesVisible") ? "active" : "";
            },

            getOrgNavigationClass: function() {
                return this.get("rolesVisible") ? "" : "active";
            },

            setRolesVisible: function() {
                this.set("rolesVisible", true);
            },

            setOrgChartVisible: function() {
                this.set("rolesVisible", false);

                $("#orgchart").data("kendoOrgChart").activate();
            },


            //
            // Employee Methods
            //
            employees: new kendo.data.DataSource({
                transport: {
                    read: { url: config.baseUrl + '/JsonGetEmployees' },
                    create: { url: config.baseUrl + '/JsonCreateEmployee' },
                    update: { url: config.baseUrl + '/JsonUpdateEmployee' },
                    destroy: { url: config.baseUrl + '/JsonDeleteEmployee' },
                    parameterMap: function (data, type) {
                        if (type == "read") {
                            return kendo.stringify({ businessPlanId: viewModelObservable.get("BusinessPlan.BusinessPlanID") });
                        } else {
                            return kendo.stringify({ employee: data });
                        }
                    }
                },
                sort: [
                    { field: "FirstName", dir: "asc" },
                    { field: "LastName", dir: "asc" }
                ],
                error: function (e) {
                    if (e.errors) alert(e.errors);
                    this.cancelChanges();
                },
                schema: {
                    model: { id: "EmployeeID" },
                    errors: function (response) {
                        if (response.Success !== true)
                            return response.Message;
                        return false;
                    },
                    data: "Data"
                },
                requestEnd: function(e) {
                    if (e.type == "destroy" || e.type == "update") {
                        viewModelObservable.get("roles").read();
                    }
                }
            }),

            setSelectedEmployees: function () {
                var employees = this.get("employees");

                employees.pushUpdate($.map(this.getRoleEmployees(), function(e) {
                    return {
                        EmployeeID: e.EmployeeID,
                        Selected: true
                    };
                }));

                this.refreshEmployeesListView();
            },

            resetSelectedEmployees: function () {
                var employees = this.get("employees");

                employees.pushUpdate($.map(employees.data(), function (e) {
                    return {
                        EmployeeID: e.EmployeeID,
                        Selected: false
                    };
                }));

                this.refreshEmployeesListView();
            },

            refreshEmployeesListView: function() {
                $("#employees").data("kendoListView").refresh();
            },

            getRoleEmployees: function () {
                return this.get("currentRole.Employees") || [];
            },

            addEmployeeInFocus: false,
            newEmployee: { FirstName: '', MiddleInitial: '', LastName: '' },

            getEmployeeManageListCssClass: function () {
                return "editor-manage-list" + (this.get("addEmployeeInFocus") ? " focused" : "");
            },

            addEmployeeFocus: function () {
                this.set("addEmployeeInFocus", true);
            },

            resetAddEmployee: function () {
                this.set("newEmployee", { FirstName: '', MiddleInitial: '', LastName: '' });
                this.set("addEmployeeInFocus", false);
                Placeholders.enable();
            },
            
            addEmployee: function () {
                var employees = this.get("employees"),
                    newEmployee = this.get("newEmployee");

                var employee = {
                    FirstName: trim(newEmployee.FirstName),
                    MiddleInitial: trim(newEmployee.MiddleInitial),
                    LastName: trim(newEmployee.LastName),
                    Selected: false,
                    BusinessPlanID: this.get("BusinessPlan.BusinessPlanID")
                };

                if (!this.validateEmployee(employee))
                    return;

                employees.add(employee);
                employees.sync();

                this.resetAddEmployee();
            },

            validateEmployee: function (employee) {
                var errors = [];

                if (employee.FirstName.length == 0)
                    errors.push("Please enter a first name");

                if (employee.LastName.length == 0)
                    errors.push("Please enter a last name");

                if (errors.length)
                    showErrorModal(errors);

                return (errors.length == 0);
            },

            saveEmployee: function(e) {
                if (!this.validateEmployee(e.model))
                    e.preventDefault();
            },

            cancelEditEmployee: function (e) {
                this.refreshEmployeesListView();
            },

            removeEmployee: function(e) {
                var employee = e.model,
                    employees = this.get("employees"),
                    message = "Are you sure you want to delete " + this.getEmployeeName(employee) + "?";

                var hasChildren = _.find(employees.data(), function(emp) { return emp.EmployeeParentID == employee.EmployeeID; }) != null;

                if (hasChildren)
                    message += "\n\nDoing so will also create orphaned subordinates within your Organizational Chart.";

                if (!confirm(message)) {
                    e.preventDefault();
                    return;
                }

                this.removeFromCollection(this.getRoleEmployees(), employee, "EmployeeID");
            },

            getEmployeeName: function(employee) {
                var name = "";

                if (employee.FirstName && employee.FirstName.length)
                    name += employee.FirstName;

                if (employee.MiddleInitial && employee.MiddleInitial.length) {
                    if (name.length)
                        name += " ";

                    name += employee.MiddleInitial;
                }

                if (employee.LastName && employee.LastName.length) {
                    if (name.length)
                        name += " ";

                    name += employee.LastName;
                }

                return name;
            },

            addRemoveEmployeeToRole: function (e) {
                var employee = e.data,
                    selected = employee.get("Selected"),
                    employees = this.getRoleEmployees();

                if (selected) {
                    employees.push(employee);
                } else {
                    this.removeFromCollection(employees, employee, "EmployeeID");
                }
            },

            orgChartChanged: function (e) {
                var employees = this.get("employees");

                $.each(e.changes, function(idx, item) {
                    var employee = employees.get(item.key),
                        parentId = item.parent <= 0 ? null : item.parent;

                    employee.set("EmployeeParentID", parentId);
                });

                employees.sync();
            },
            

            //
            // Helper Methods
            //
            saveEditItem: function (e) {
                var item = e.model,
                    itemCollection = item.parent();

                var match = _.find(itemCollection, function(i) {
                     return i.Name.toLowerCase() == item.Name.toLowerCase() && i.uid != item.uid;
                });

                if (match) {
                    e.preventDefault();
                    showErrorModal("An item with this name already exists.  Please enter a unique name.");
                }
            },

            removeEditItem: function(e) {
                if (!confirm("Are you sure you want to delete " + e.model.Name + "?"))
                    e.preventDefault();
            },

            exists: function(item, itemCollection, property) {
                var foundItem = _.find(itemCollection, function(i) {
                    return i[property].toLowerCase() === item[property].toLowerCase();
                });

                if (foundItem != null)
                    showErrorModal("An item with this name already exists.  Please enter a unique name.");

                return foundItem != null;
            },

            indexOf: function(collection, filter) {
                for (var i = 0; i < collection.length; i++) {
                    if (filter(collection[i], i, collection))
                        return i;
                }
                return -1;
            },

            removeFromCollection: function(collection, item, property) {
                var index = this.indexOf(collection, function (i) { return item[property] === i[property]; });

                if (index != -1)
                    collection.splice(index, 1);
            }
        };

        function init(options) {
            $element = $("#main");
            $wizard = $("#editor");
            $autoSaveMessages = $(".auto-save-wrap");
            $tabs = $("#tab-nav");
            $objectiveDialog = $("#objective-dialog");
            $roleDialog = $("#role-dialog");
            $autoSaveLabel = $("#auto-save");

            // Defaults for ajax calls
            $.ajaxSetup({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false
            });

            // Update config with passed in options
            $.extend(config, options);

            // Initialize our view model as an observable
            viewModelObservable = kendo.observable(viewModel);

            // Set the year of the plan
            viewModelObservable.set("selectedYear", config.selectedYear);

            // Watch for specific changes 
            viewModelObservable.bind("change", observableChanged);

            // Get the data
            viewModelObservable.populate(function () {
                
                // Bind observable to container and let kendo manage the underlying data
                kendo.bind($element, viewModelObservable);

                // Initialize textareas that grow automatically
                $("[data-role='flextext']").flexText();

                // Show it
                $wizard.fadeIn();

                // Init auto-save timer
                var timer = new portal.timer();

                timer.init({
                    interval: portal.config.autoSave.interval,
                    timeout: function() {
                        if (viewModelObservable.get("isDirty") === true)
                            viewModelObservable.saveBusinessPlan();
                    }
                });
            });

            // Init tooltip
            $autoSaveLabel.tooltip({
                html: true
            });

            // Custom scroll bars for swots
            $(".nano").nanoScroller({ alwaysVisible: true });

            // Check for changes before navigating away
            $(window).on('beforeunload', function () {
                if (viewModelObservable.get("isDirty") === true)
                    return "You have unsaved changes to this Business Plan.";
            });

            // Init session manager
            portal.sessionMonitor.init({
                sessionText: "Business-Planning Editor session"
            });

            // Init error dialog
            errorWindow = $("<div><div class='error-message'></div><button class='btn btn-primary pull-right'>Ok</button></div>").kendoWindow({ title: "", resizable: false, modal: true });
            errorWindow.find("button").on("click", function () {
                errorWindow.data("kendoWindow").close();
                errorWindow.find(".error-message").empty();
            });
        }

        function showErrorModal(errors) {
            if (typeof errors == "string")
                errors = [errors];

            if (errors.length <= 0)
                return;

            var message = "<ul>";
            $.each(errors, function(idx, error) {
                message += kendo.format("<li>{0}</li>", error);
            });
            message += "</ul>";

            errorWindow.find(".error-message").html(message);
            errorWindow.data("kendoWindow").setOptions({ title: "Error", width: 400 });
            errorWindow.data("kendoWindow").center().open();
        }

        function observableChanged(e) {
            if (e.field == null)
                return;

            if (e.field == "swots" || e.field == "selectedTabIndex") {
                window.setTimeout(refreshScrollBars, 300);
            }

            if (e.field.slice(0, 12) == "BusinessPlan") {
                e.sender.set("isDirty", true);
            }
        }

        function refreshScrollBars() {
            $(".nano").nanoScroller({ alwaysVisible: true });
        }

        return {
            init: init
        };

    }();


}(this.portal = this.portal || {}, jQuery, _));