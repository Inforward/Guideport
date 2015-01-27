(function () {

    var kendo = window.kendo,
        ui = kendo.ui,
        Widget = ui.Widget,
        DATABINDING = "dataBinding",
        DATABOUND = "dataBound",
        CHANGE = "change";

    var OrgChart = Widget.extend({

        init: function (element, options) {
            var that = this;

            kendo.ui.Widget.fn.init.call(that, element, options);

            if (that._unsupported())
                return;

            that._chart();
            that._dataSource();
        },

        options: {
            name: "OrgChart",
            readonly: false,
            enabled: true,
            autoBind: true,
            autoScale: true,
            keyField: "key",
            parentField: "parent",
            nameField: "name",
            titleField: "title",
            minHeight: 400
        },

        events: [
            DATABINDING,
            DATABOUND,
            CHANGE
        ],

        refresh: function (e) {
            var that = this;

            if (that._unsupported())
                return;

            that.trigger(DATABINDING);

            var data = $.map(that.dataSource.data(), function (item) {
                return {
                    key: item[that.options.keyField],
                    parent: item[that.options.parentField] || 0,
                    oldParent: item[that.options.parentField] || 0,
                    name: item[that.options.nameField],
                    title: item[that.options.titleField]
                };
            });

            that.diagram.model = new go.TreeModel(data);

            //that.trigger(DATABOUND);
        },

        activate: function() {
            if (this.diagram)
                this.diagram.requestUpdate();
        },

        items: function() {
            return this.element.children();
        },

        setDataSource: function(dataSource) {
            this.options.dataSource = dataSource;
            this._dataSource();
        },

        _subscribe: function() {
            this._unsubscribe();

            this._refreshHandler = $.proxy(this.refresh, this);

            if (this.dataSource) {
                this.dataSource.bind(CHANGE, this._refreshHandler);
            }
        },

        _unsubscribe: function() {
            if (this.dataSource && this._refreshHandler) {
                this.dataSource.unbind(CHANGE, this._refreshHandler);
            }
        },

        _unsupported: function() {
            return (typeof (window.go) == "undefined");
        },

        _change: function () {
            if (this._unsupported())
                return;

            var that = this,
                changes = [];

            $.each(that.diagram.model.nodeDataArray, function (idx, node) {
                if (node.parent != node.oldParent) {
                    changes.push({
                        key: node.key,
                        parent: node.parent
                    });
                    node.oldParent = node.parent;
                }
            });

            if (changes.length) {
                that._unsubscribe();

                that.trigger(CHANGE, { changes: changes });

                that._subscribe();
            }
        },

        _dataSource: function () {
            var that = this;

            that._unsubscribe();

            that.dataSource = kendo.data.DataSource.create(that.options.dataSource);

            that._subscribe();

            if (that.options.autoBind)
                that.dataSource.fetch();
        },

        _chart: function () {

            if (this._unsupported())
                return;

            var that = this,
                $ = go.GraphObject.make,
                scaling = that.options.autoScale ? go.Diagram.Uniform : go.Diagram.None;

            var myDiagram = $(go.Diagram, that.element[0], {
                allowCopy: false,
                allowInsert: false,
                allowTextEdit: false,
                allowZoom: false,
                isEnabled: that.options.enabled,
                isReadOnly: that.options.readonly,

                autoScale: scaling,
                initialContentAlignment: go.Spot.TopCenter,

                // make sure users can only create trees
                validCycle: go.Diagram.CycleDestinationTree,

                // users can select only one part at a time
                maxSelectionCount: 1,

                layout: $(go.TreeLayout, {
                    treeStyle: go.TreeLayout.StyleLastParents,
                    arrangement: go.TreeLayout.ArrangementHorizontal,

                    // properties for most of the tree:
                    angle: 90,
                    layerSpacing: 35,

                    // properties for the "last parents":
                    alternateAngle: 90,
                    alternateLayerSpacing: 35,
                    alternateAlignment: go.TreeLayout.AlignmentBus,
                    alternateNodeSpacing: 20
                }),
                "undoManager.isEnabled": false
            });
           

            var levelColors = ["#008dd5", "#5a76ca", "#009766", "#a6a7a9", "#787775"];

            // override TreeLayout.commitNodes to also modify the background brush based on the tree depth level
            myDiagram.layout.commitNodes = function () {

                go.TreeLayout.prototype.commitNodes.call(myDiagram.layout);

                // Go through all of the vertexes and set their corresponding node's Shape.fill to a brush dependent on the TreeVertex.level value
                myDiagram.layout.network.vertexes.each(function (v) {
                    if (v.node) {
                        var level = v.level % (levelColors.length),
                            color = levelColors[level],
                            shape = v.node.findObject("SHAPE");

                        if (shape)
                            shape.fill = $(go.Brush, go.Brush.Linear, { 0: color, 1: color, start: go.Spot.Left, end: go.Spot.Right });
                    }
                });

                // Process changes that may have occured
                that._change();
            }

            // this is used to determine feedback during drags
            function mayWorkFor(node1, node2) {
                return !(!(node1 instanceof go.Node) || (node1 === node2) || node2.isInTreeOf(node1));
            }

            function textStyle() {
                return { font: "14px  Arial, Verdana", stroke: "white" };
            }

            myDiagram.nodeTemplate = $(go.Node, "Auto", {
                    doubleClick: null,
                    deletable: false
                },{
                    mouseDragEnter: function (e, node, prev) {
                        var diagram = node.diagram,
                            selnode = diagram.selection.first(),
                            shape = node.findObject("SHAPE");

                        if (!mayWorkFor(selnode, node))
                            return;
                        
                        if (shape) {
                            shape._prevFill = shape.fill;
                            shape.fill = "darkred";
                        }
                    },
                    mouseDragLeave: function (e, node, next) {
                        var shape = node.findObject("SHAPE");

                        if (shape && shape._prevFill)
                            shape.fill = shape._prevFill;
                    },
                    mouseDrop: function (e, node) {
                        var diagram = node.diagram,
                            selnode = diagram.selection.first();

                        if (mayWorkFor(selnode, node)) {
                            var link = selnode.findTreeParentLink();

                            if (link !== null) {
                                link.fromNode = node;
                            } else {
                                diagram.toolManager.linkingTool.insertLink(node, node.port, selnode, selnode.port);
                            }
                        }
                    }
                },

                // for sorting, have the Node.text be the data.name
                new go.Binding("text", "name"),

                // bind the Part.layerName to control the Node's layer depending on whether it isSelected
                new go.Binding("layerName", "isSelected", function (sel) { return sel ? "Foreground" : ""; }).ofObject(),

                // define the node's outer shape
                $(go.Shape, "RoundedRectangle", {
                    name: "SHAPE",
                    fill: "white",
                    stroke: null,
                    portId: "",
                    fromLinkable: false,
                    toLinkable: false,
                    cursor: "pointer"
                }),

                $(go.Panel, "Horizontal", $(go.Picture, {
                        maxSize: new go.Size(39, 50),
                        margin: new go.Margin(6, 8, 6, 10),
                    },
                    new go.Binding("source", "key", function() { return "/Assets/Images/person.png"; })),

                  // define the panel where the text will appear
                  $(go.Panel, "Table", {
                        maxSize: new go.Size(150, 999),
                        margin: new go.Margin(6, 10, 0, 3),
                        defaultAlignment: go.Spot.Left
                    },
                    $(go.RowColumnDefinition, { column: 2, width: 4 }),

                    $(go.TextBlock, textStyle(), {
                        row: 0,
                        column: 0,
                        columnSpan: 5,
                        font: "14px  Arial, Verdana",
                        editable: false,
                        isMultiline: false,
                        minSize: new go.Size(10, 16)
                      },
                      new go.Binding("text", "name").makeTwoWay()),

                    $(go.TextBlock, textStyle(), {
                        row: 1,
                        column: 0,
                        columnSpan: 5,
                        font: "10px  Arial, Verdana",
                        editable: false,
                        isMultiline: false,
                        minSize: new go.Size(10, 14),
                        margin: new go.Margin(0, 0, 0, 0)
                      },
                      new go.Binding("text", "title").makeTwoWay())
                  )
                )
            );

            // define the Link template
            myDiagram.linkTemplate = $(go.Link, go.Link.Orthogonal, {
                  corner: 5,
                  relinkableFrom: true,
                  relinkableTo: true
                }, $(go.Shape, {
                    strokeWidth: 4,
                    stroke: "#b9c5d3"
                }));  // the link shape


            that.diagram = myDiagram;

        }

    });

    ui.plugin(OrgChart);

})(jQuery);