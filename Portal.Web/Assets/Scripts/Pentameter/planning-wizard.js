
(function (one, $, kendo, _) {
    
    portal.planningWizard = function () {
        var viewModelObservable,

            config = {
                baseUrl: '/',
                returnUrl: '/pentameter/succession-planning/dashboard'
            };

        
        //
        // View model definitions
        //
        var viewModel = {

            isSaving: null,
            isDirty: false,
            
            percentCompleteText: function () {
                return kendo.format("{0:N0}%", this.getPercentComplete());
            },
            
            getPercentComplete: function () {
                var totalWeight = 0;

                $.each(this.get("Phases"), function (i, phase) {
                    $.each(phase.get("Steps"), function (y, step) {
                        if (step.isComplete())
                            totalWeight += step.StepWeight;
                    });
                });

                return Math.round(totalWeight * 100);
            },

            getSelectedPhase: function() {
                return _.find(this.get("Phases"), function (p) { return p.Selected; });
            },

            toProgressData: function() {
                var progress = {
                    ProgressId: this.get("ProgressId"),
                    PercentComplete: this.getPercentComplete(),
                    CurrentPhaseId: this.getSelectedPhase().PhaseId,
                    Phases: []
                };

                progress.Phases = $.map(this.get("Phases"), function(phase) {
                    return phase.toProgressData();
                });

                return progress;
            },

            isWizardComplete: function() {
                return this.getPercentComplete() >= 100;
            },

            actionItemClicked: function (e) {
                var item = e.data,
                    complete = item.get("Complete");

                item.set("Complete", !complete);

                $.each(this.get("Phases"), function (idx, phase) {
                    phase.trigger("change", { field: "phaseNavClass" });
                });

                this.set("isDirty", true);
            },

            getPromptCacheKey: function() {
                return "planning." + this.get("ProgressId") + ".prompt";
            },

            completeClicked: function () {

                this.saveProgress(function () {

                    var cacheKey = this.getPromptCacheKey(),
                        prompted = localStorage.getItem(cacheKey) || false;

                    if (!prompted) {
                        localStorage.setItem(cacheKey, true);

                        this.showCompleteMessage(function () { window.location = config.returnUrl; });

                    } else {
                        window.location = config.returnUrl;
                    }
                });
            },

            showCompleteMessage: function(callback) {
                var $wnd = $("<div class='confirm-window'><p>" + this.get("CompleteMessage") + "</p><button class='k-button'>Ok</button></div>");

                $wnd.kendoWindow({
                    title: "Congratulations!",
                    resizable: false,
                    modal: true
                });

                $wnd.find("button").on("click", function () {
                    $wnd.data("kendoWindow").close().destroy();
                    $wnd.remove();

                    if (callback)
                        callback.call(this);
                });

                $wnd.data("kendoWindow").center().open();
            },

            saveProgress: function(callback) {
                var progress = this.toProgressData(),
                    that = this;

                that.set("isSaving", true);

                portal.dataManager.send({
                    url: config.baseUrl + "/JsonSavePlanningProgress",
                    data: kendo.stringify({ progress: progress }),
                    success: function (data) {
                        that.set("isDirty", false);

                        if (!that.isWizardComplete())
                            localStorage.removeItem(that.getPromptCacheKey());

                        if (callback && typeof(callback) == "function")
                            callback.call(that, data);
                    }
                }).always(function () {
                    that.set("isSaving", false);
                });
            }
        },

        phaseViewModel = {
            
            phaseNavClass: function () {
                var cssClass = "";

                if (this.get("Selected"))
                    cssClass += "active";

                if (this.isEnabled() && this.isComplete())
                    cssClass += " complete";

                if (this.isEnabled())
                    cssClass += " enabled";
                
                return cssClass;
            },
            
            nextPhaseTooltip: function() {
                return this.isComplete() ? "" : "You must complete all Steps before you may proceed to the next phase.";
            },
            
            nextPhase: function() {
                var sortOrder = this.get("SortOrder"),
                    phases = this.getPhases();
                
                return _.find(phases, function (p) { return p.SortOrder > sortOrder; });
            },

            previousPhase: function() {
                var that = this,
                    previousIndex = 0,
                    phases = this.getPhases();

                $.each(phases, function (idx, phase) {
                    if (phase.PhaseId == that.PhaseId) {
                        previousIndex = idx - 1;
                        return;
                    }
                });

                return previousIndex >= 0 ? phases[previousIndex] : null;
            },

            previousPhases: function() {
                var sortOrder = this.get("SortOrder");

                return _.filter(this.getPhases(), function (p) { return p.SortOrder < sortOrder; });
            },
            
            isComplete: function () {
                return _.filter(this.get("Steps"), function (s) { return s.isComplete(); }).length == this.get("Steps").length;
            },
            
            isEnabled: function () {
                var previousPhases = this.previousPhases(),
                    completePhases = _.filter(previousPhases, function (p) { return p.isComplete(); });

                return previousPhases.length == completePhases.length;
            },

            isFirstPhase: function () {
                return this.getPhases()[0].PhaseId == this.PhaseId;
            },

            isLastPhase: function () {
                var phases = this.getPhases(),
                    length = phases.length;

                return phases[length - 1].PhaseId == this.PhaseId;
            },
            
            selectPhase: function () {
                var phaseId = this.get("PhaseId");
                
                if (!this.isEnabled() || this.get("Selected"))
                    return;
                
                $.each(this.getPhases(), function (idx, phase) {
                    phase.set("Selected", (phase.PhaseId == phaseId));
                });
            },

            getWizard: function() {
                return this.parent().parent();
            },

            getPhases: function() {
                return this.getWizard().get("Phases");
            },

            toProgressData: function () {
                var phase = {
                    PhaseId: this.get("PhaseId"),
                    Selected: this.get("Selected"),
                    Steps: []
                };

                phase.Steps = $.map(this.get("Steps"), function(step) {
                    return step.toProgressData();
                });

                return phase;
            }
        },

        stepViewModel = {

            goToPrevious: function () {
                var previousStep = this.previousStep(),
                    previousPhase = this.getPhase().previousPhase();

                if (previousStep) {
                    previousStep.selectStep();

                } else if (previousPhase) {
                    var steps = previousPhase.get("Steps"),
                        lastStep = steps[steps.length - 1];

                    lastStep.selectStep();
                    previousPhase.selectPhase();

                    this.scrollStepIntoView();
                }
            },

            goToNext: function() {
                var nextStep = this.nextStep(),
                    nextPhase = this.getPhase().nextPhase();

                if (nextStep) {
                    nextStep.selectStep();

                } else if(nextPhase) {
                    var steps = nextPhase.get("Steps"),
                        firstStep = steps[0];

                    firstStep.selectStep();
                    nextPhase.selectPhase();

                    this.scrollStepIntoView();
                }
            },

            isNextEnabled: function() {
                var nextStep = this.nextStep(),
                    nextPhase = this.getPhase().nextPhase();

                if (nextStep)
                    return true;

                if (nextPhase && nextPhase.isEnabled())
                    return true;

                return false;
            },

            isPreviousEnabled: function() {
                var previousStep = this.previousStep(),
                    previousPhase = this.getPhase().previousPhase();

                if (previousStep)
                    return true;

                if (previousPhase && previousPhase.isEnabled())
                    return true;

                return false;
            },

            isVeryLastStep: function() {
                return this.getPhase().isLastPhase() && (this.nextStep() == null);
            },

            getSteps: function () {
                return this.getPhase().get("Steps");
            },

            nextStep: function () {
                var sortOrder = this.get("StepNo"),
                    steps = this.getSteps();

                return _.find(steps, function (s) { return s.StepNo > sortOrder; });
            },

            previousStep: function () {
                var that = this,
                    previousIndex = 0,
                    steps = this.getSteps();

                $.each(steps, function (idx, step) {
                    if (step.StepId == that.StepId) {
                        previousIndex = idx - 1;
                        return;
                    }
                });

                return previousIndex >= 0 ? steps[previousIndex] : null;
            },

            stepNavCssClass: function () {
                var cssClass = "";

                if (this.get("Selected") === true)
                    cssClass += "active";

                if (this.isComplete())
                    cssClass += " complete";
                else
                    cssClass += this.completedActionItems().length > 0 ? " in-progress" : " not-started";

                return cssClass;
            },
            
            stepStatusText: function () {
                if (this.isComplete())
                    return "Complete";
                
                return this.completedActionItems().length > 0 ? "In Progress" : "";
            },
            
            completedActionItems: function () {
                return _.filter(this.get("ActionItems"), function (item) { return item.Complete; });
            },
            
            isComplete: function () {
                return this.get("ActionItems").length == this.completedActionItems().length;
            },
            
            selectStep: function () {
                var stepId = this.get("StepId");
                
                if (this.get("Selected"))
                    return;

                $.each(this.getPhase().get("Steps"), function (idx, step) {
                    step.set("Selected", (step.StepId == stepId));
                });

                this.scrollStepIntoView();
            },

            scrollStepIntoView: function() {
                $(".phase-container").scrollintoview({ duration: "slow" });
            },
            
            actionItemHeaderText: function () {
                return "Action Item" + (this.get("ActionItems").length > 1 ? "s" : "") + ":";
            },

            notesChanged: function() {
                this.getPhase().getWizard().set("isDirty", true);
            },

            getPhase: function() {
                return this.parent().parent();
            },
            
            toProgressData: function () {
                var step = {
                    StepId: this.get("StepId"),
                    PhaseId: this.get("PhaseId"),
                    Selected: this.get("Selected"),
                    Notes: this.get("Notes"),
                    StepWeight: this.get("StepWeight"),
                    ActionItems: []
                };

                step.ActionItems = $.map(this.get("ActionItems"), function(item) {
                    return item.toProgressData();
                });

                return step;
            }
        },
            
        actionItemViewModel = {

            actionItemCheckBoxClass: function() {
                return "checkbox " + (this.get("Complete") === true ? "checked" : "");
            },

            actionItemCssClass: function() {
                return this.get("Complete") ? "complete" : "";
            },

            resourcesChanged: function () {
                portal.dataManager.send({
                    url: config.baseUrl + '/JsonUpdateActionItem',
                    data: kendo.stringify(this)
                });
            },

            getStep: function () {
                return this.parent().parent();
            },
            
            toProgressData: function () {
                return {
                    ActionItemId: this.get("ActionItemId"),
                    StepId: this.get("StepId"),
                    Complete: this.get("Complete"),
                    CompleteDate: this.get("Complete") ? new Date() : null
                };
            }
        };

        function init(options) {

            // Update config with passed in options
            $.extend(config, options);

            // Grab current progress and initialize the page
            portal.dataManager.send({
                url: config.baseUrl + "/JsonGetPlanningProgress",
                success: function (data) {
                    initializeWizard(data);
                }
            });
        }
        
        function initializeWizard(wizard) {
            
            // Combine our raw data models with our view models
            $.each(wizard.Phases, function (i, phase) {
                $.extend(true, phase, phaseViewModel);
                
                $.each(phase.Steps, function (y, step) {
                    $.extend(true, step, stepViewModel);
                    
                    $.each(step.ActionItems, function (x, item) {
                        $.extend(true, item, actionItemViewModel);
                    });
                });
            });

            // Merge our workflow data with the pre-defined view model and initialize observable
            viewModelObservable = kendo.observable($.extend(true, viewModel, wizard));
            
            // Bind observable to container and let kendo do the heavy-lifting
            kendo.bind($("#workflow"), viewModelObservable);

            // Show it
            $("#workflow").fadeIn();

            // Init auto-save timer
            var timer = new portal.timer();

            timer.init({
                interval: portal.config.autoSave.interval,
                timeout: function () {
                    if (viewModelObservable.get("isDirty") === true)
                        viewModelObservable.saveProgress();
                }
            });

            // Check for changes before navigating away
            $(window).on('beforeunload', function () {
                if (viewModelObservable.get("isDirty") === true)
                    return "You have unsaved changes.";
            });

            // Init session manager
            portal.sessionMonitor.init({
                sessionText: "Planning session"
            });
        }
        
        return {
            init: init
        };

    }();

}(this.portal = this.portal || {}, jQuery, kendo, _));