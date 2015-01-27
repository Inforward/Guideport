
(function (portal, $, _) {

    portal.survey = function () {
        var confirmWindow,
            currentSurvey,
            surveyObservable;

        var config = {
            surveyName: null,
            readOnly: false
        };

        var answerTypes = {
            Text : "Text",
            Select : "Select",
            MultiSelect : "MultiSelect",
            None : "None"
        },
        
        inputTypes = {
            None : "None",
            CheckBoxList : "CheckBoxList",
            RadioButtonList : "RadioButtonList",
            Date : "Date",
            Group : "Group",
            MultiText : "MultiText",
            Number : "Number",
            Percent : "Percent",
            Select : "Select",
            Text : "Text",
            TextArea : "TextArea"
        },
            
        surveyViewModel = {

            isSaving: null,

            findQuestion: function (id) {
                var question;

                $.each(this.get("Pages"), function (idx, page) {
                    question = page.findQuestion(id);
                    if (question)
                        return false;
                });

                return question;
            },
            
            getSelectedPage: function() {
                var pages = this.get("Pages"),
                    selectedPage = _.find(pages, function (p) { return p.isVisible(); });
                
                return selectedPage || pages[0];
            },
            
            findPage: function(id) {
                return _.find(this.get("Pages"), function (p) { return p.Id == id; });
            },
            
            validate: function () {
                var isValid = true,
                    firstErrorPage = null;
                
                $.each(this.get("Pages"), function (idx, page) {
                    if (page.get("IsVisible") && !page.validate()) {
                        isValid = false;
                        firstErrorPage = firstErrorPage || page;
                    }
                });
                
                if (!isValid) {
                    this.getSelectedPage().set("IsSelected", false);
                    firstErrorPage.set("IsSelected", true);
                    scrollErrorIntoView();
                }
                
                return isValid;
            },
            
            getResponse: function () {
                var response = {
                    SurveyId: this.get("Id"),
                    SurveyName: this.get("Name"),
                    SelectedPageId: this.getSelectedPage().Id,
                    Answers: []
                };
                
                if (response.SelectedPageId <= 0)
                    response.SelectedPageId = this.get("Pages")[0].Id;
                
                $.each(this.get("Pages"), function (idx, page) {
                    $.merge(response.Answers, page.getResponseAnswers());
                });

                return response;
            }
        },

        surveyPageViewModel = {

            isVisible: function () {
                return this.get("IsVisible") && this.get("IsSelected");
            },
            
            isReviewVisible: function () {
                return this.get("IsVisible") && this.get("Name") != this.getSurvey().get("ReviewTabText");
            },

            tabName: function() {
                return this.get("Name");
            },

            tabClass: function () {
                var cssClass = "k-item k-state-default";

                if (this.isVisible())
                    cssClass += " k-tab-on-top k-state-active";

                return cssClass;
            },

            tabContentClass: function () {
                var cssClass = "survey-page-wrap k-content";

                if (this.isVisible())
                    cssClass += " k-state-active";

                return cssClass;
            },

            selectTab: function () {
                var id = this.get("Id");

                if (!this.get("IsVisible"))
                    return;

                $.each(this.getAllPages(), function (idx, page) {
                    page.set("IsSelected", (page.Id == id));
                });
            },

            findQuestion: function (id) {
                return _.find(this.get("Questions"), function (q) { return q.Id == id; });
            },
            
            validate: function(showFirstError) {
                var isValid = true;
                
                $.each(this.get("Questions"), function (idx, question) {
                    if (question.get("IsVisible") && !question.validate())
                        isValid = false;
                });
                
                if (!isValid && showFirstError === true)
                    scrollErrorIntoView();
                
                return isValid;
            },
            
            getAllPages: function () {
                return this.getSurvey().get("Pages");
            },

            getAllPagesWithScores: function () {
                var reviewText = this.getSurvey().get("ReviewTabText");

                return _.filter(this.getAllPages(), function (p) { return p.Name != reviewText; });
            },

            hasScoreDescriptions: function () {
                var scoreDescriptionFound = false;

                $.each(this.getSurvey().get("Pages"), function (index, value) {
                    if (value.ScoreDescription != "" && value.ScoreDescription != null)
                        scoreDescriptionFound = true;
                });

                return scoreDescriptionFound;
            },

            getResponseAnswers: function () {
                var answers = [];
                
                $.each(this.get("Questions"), function (idx, question) {
                    $.merge(answers, question.getResponseAnswers());
                });
                
                return answers;
            },
            
            getSurvey: function() {
                return this.parent().parent();
            },
            
            getPdfUrl: function () {
                var survey = this.getSurvey(),
                    surveyName = survey.get("Name");
                
                return kendo.format("{0}pdf/ExecuteUrl?url=/Survey/Print/{1}&title={1}&type=Survey", portal.rootUrl, surveyName);
            },

            resetPage: function (e) {
                e.preventDefault();

                var thisPage = this,
                    hasTrigger = false,
                    currentPage = _.find(currentSurvey.Pages, function (p) { return p.Id == thisPage.Id; });
                
                $.each(currentPage.Questions, function (idx, currentQuestion) {
                    var question = thisPage.findQuestion(currentQuestion.Id);
                    
                    question.set("IsVisible", currentQuestion.IsVisible);
                    question.set("Number", currentQuestion.Number);
                    question.set("Answer", currentQuestion.Answer);
                    
                    if (!hasTrigger) hasTrigger = question.get("HasTrigger");

                    $.each(currentQuestion.PossibleAnswers, function (y, currentAnswer) {
                        var answer = question.findPossibleAnswer(currentAnswer.Id);
                        answer.set("IsSelected", currentAnswer.IsSelected);
                        
                        if (question.InputType == inputTypes.RadioButtonList)
                            $("#" + answer.uid).prop("checked", currentAnswer.IsSelected);
                    });
                });
                
                if (hasTrigger)
                    processTrigger();
            },

            savePage: function () {
                var thisPage = this;
                
                saveResponse(function () {
                    thisPage.validate();
                });
            },

            nextPage: function () {
                var thisPage = this;
                
                saveResponse(function () {
                    if (thisPage.validate(true)) {
                        var nextPage = _.find(thisPage.getAllPages(), function (p) { return p.PageOrder > thisPage.PageOrder && p.get("IsVisible") === true; });

                        if (nextPage) {
                            thisPage.set("IsSelected", false);
                            nextPage.set("IsSelected", true);
                            $(window).scrollTop(0);
                        }
                    }
                });
            },
            
            completeSurvey: function () {
                if (this.getSurvey().validate()) {
                    saveResponse(function (data) {
                        showCompleteConfirmation(data.survey.CompleteMessage, "Complete", 450, data.survey.CompleteRedirectUrl);
                    }, true);
                }
            }
        },

        surveyQuestionViewModel = {
            
            hasError: false,
            errorMessage: null,
            
            questionClass: function() {
                var cssClass = "q-wrap " + this.get("Layout");
                
                if (this.get("hasError"))
                    cssClass += " error";
                
                return cssClass;
            },

            questionInputClass: function () {
                return this.get("InputType").toLowerCase();
            },
            
            isReviewVisible: function () {
                return this.get("IsVisible") && this.get("InputType") != inputTypes.None;
            },
            
            getReviewAnswer: function () {
                var answer = this.get("Answer");
                return (answer != "None Selected" ? answer : "");
            },
            
            getPage: function() {
                return this.parent().parent();
            },

            selectAnswerChanged: function (e) {
                var $elem = $(e.currentTarget);

                this.set("Answer", $elem.find("option:selected").val());
                
                this.clearError();

                if (this.get("HasTrigger"))
                    processTrigger();
            },
            
            getSelectedAnswers: function() {
                return _.filter(this.get("PossibleAnswers"), function (a) { return a.IsSelected; });
            },
            
            getResponseAnswers: function() {
                var inputType = this.get("InputType"),
                    answerType = this.get("AnswerType"),
                    questionId = this.get("Id"),
                    pageId = this.getPage().get("Id"),
                    answer = $.trim(this.get("Answer"));

                if (this.get("IsVisible")) {
                    if (inputType == inputTypes.MultiText) {
                        return $.map(this.get("Answers"), function (a) {
                            return {
                                SurveyQuestionId: questionId,
                                SurveyPageId: pageId,
                                Answer: a.Value
                            };
                        });
                    } else if (answerType == answerTypes.Text || inputType == inputTypes.Select) {
                        if (answer.length && answer != "None Selected") {
                            return [{
                                SurveyQuestionId: questionId,
                                SurveyPageId: pageId,
                                Answer: answer
                            }];
                        }
                    } else if (inputType == inputTypes.CheckBoxList || inputType == inputTypes.RadioButtonList) {
                        return $.map(this.getSelectedAnswers(), function (a) {
                            return {
                                SurveyQuestionId: questionId,
                                SurveyPageId: pageId,
                                Answer: a.get("Text")
                            };
                        });
                    } 
                }
                
                return [];
            },
            
            findPossibleAnswer: function(id) {
                return _.find(this.get("PossibleAnswers"), function (a) { return a.Id == id; });
            },
            
            setError: function(errorMessage) {
                this.set("hasError", true);
                this.set("errorMessage", errorMessage);
            },
            
            clearError: function () {
                this.set("hasError", false);
                this.set("errorMessage", null);
            },
            
            validate: function() {
                var isValid = true;
                
                if (this.get("IsRequired")) {
                    isValid = (this.getResponseAnswers().length > 0);
                }
                
                if(!isValid) {
                    this.setError("Answer is required.");
                } else {
                    this.clearError();
                }
                
                return isValid;
            }
        },

        surveyAnswerViewModel = {

            answerChanged: function () {
                this.getQuestion().clearError();

                if (this.get("IsTrigger"))
                    processTrigger();
            },
            
            getQuestion: function() {
                return this.parent().parent();
            },

            radioAnswerChanged: function () {
                var answerId = this.get("Id"),
                    question = this.getQuestion();

                $.each(question.get("PossibleAnswers"), function (idx, answer) {
                    answer.set("IsSelected", answer.Id == answerId);
                });

                this.answerChanged();
            }
        };

        function init(options) {
            $.extend(config, options);
            
            $.ajaxSetup({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false
            });

            $.ajax({
                url: "/survey/get-survey/" + config.surveyName,
                success: function (survey) {
                    initializeSurvey(survey);
                }
            });
        }

        function initializeSurvey(survey) {
            var $surveyContent = $("#survey-wrap");
            
            // Store copy for use in reseting pages (clone it)
            currentSurvey = $.extend(true, {}, survey);

            // Combine our raw data models with our view models
            $.each(survey.Pages, function (i, page) {
                $.extend(true, page, surveyPageViewModel);

                $.each(page.Questions, function (y, question) {
                    $.extend(true, question, surveyQuestionViewModel);

                    $.each(question.PossibleAnswers, function (x, answer) {
                        $.extend(true, answer, surveyAnswerViewModel);
                    });
                });
            });

            // Merge our survey data with the pre-defined view model and initialize observable
            surveyObservable = kendo.observable($.extend(true, survey, surveyViewModel));

            // Bind observable to container and let kendo build the UI and manage the underlying data
            kendo.bind($surveyContent, surveyObservable);

            // Now show it
            $surveyContent.fadeIn("slow");

            // Attach plugin to ensure numeric only input
            $surveyContent.find("input.numeric, input.percent").numericOnly();
            
            // Attach handler to 'View as PDF' link to save the response to ensure the PDF has the latest answers
            $("#pdf-link").on("click", function () {
                saveResponse();
            });
            
            // Initialize complete confirmation dialog
            confirmWindow = $("<div><div class='confirm-message'></div><button class='k-button'>Ok</button></div>").kendoWindow({
                title: "Complete",
                resizable: false,
                modal: true,
                width: 450
            });

            confirmWindow.find("button").on("click", function () {
                confirmWindow.data("kendoWindow").close();
            });

            // Set datasource on multi-select inputs
            $("[data-role='multiselect']").each(function () {
                $(this).data("kendoMultiSelect").setDataSource({
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: "/Geo/JsonGetLocations"
                        },
                        parameterMap: function (options) {
                            if (options.filter) {
                                return kendo.stringify({ searchPattern: options.filter.filters[0].value });
                            }
                        }
                    }
                });
            });
            
            // Set read-only if configured
            if(config.readOnly) {
                $surveyContent.find("input").prop("disabled", true).prop("checked", false);
                $surveyContent.find(".button-wrap").hide();
                $(".k-link:contains('Review')").parent().hide();
            }

            // Init auto-save timer
            var timer = new portal.timer();

            timer.init({
                interval: portal.config.autoSave.interval,
                timeout: function () {
                    saveResponse();
                }
            });

            // Init session monitor
            portal.sessionMonitor.init();
        }

        function processTrigger() {
            $.ajax({
                url: "/survey/process-trigger",
                data: kendo.stringify(surveyObservable.getResponse()),
                async: false,
                success: function (survey) {
                    
                    $.each(survey.Pages, function (idx, page) {
                        var currentPage = surveyObservable.findPage(page.Id);
                        currentPage.set("IsVisible", page.IsVisible);

                        $.each(page.Questions, function (x, question) {
                            var currentQuestion = currentPage.findQuestion(question.Id);
                            currentQuestion.set("IsVisible", question.IsVisible);
                            currentQuestion.set("Number", question.Number);

                            $.each(question.PossibleAnswers, function (y, answer) {
                                var a = currentQuestion.findPossibleAnswer(answer.Id);
                                if (a) {
                                    a.set("IsSelected", answer.IsSelected);
                                    
                                    if (currentQuestion.get("InputType") == inputTypes.RadioButtonList)
                                        $("#" + a.uid).prop("checked", answer.IsSelected);
                                }
                            });
                        });
                    });
                }
            });
        }
        
        function processServerValidationErrors(validationErrors) {
            $.each(validationErrors, function (idx, error) {
                surveyObservable.findQuestion(error.SurveyQuestionID).setError(error.ErrorMessage);
            });
            
            scrollErrorIntoView();
        }
        
        function scrollErrorIntoView() {
            $(".q-error-wrap span:visible").first().parentsUntil(".q-wrap").parent().scrollintoview();
        }
        
        function showCompleteConfirmation(message, title, width, redirectUrl) {
            var kendoWindow = confirmWindow.data("kendoWindow");
            
            confirmWindow.find(".confirm-message").html(message);
            kendoWindow.setOptions({ title: title, width: width });
            kendoWindow.center().open();

            if (redirectUrl) {
                kendoWindow.close = function () {
                    window.location = redirectUrl;
                }
            }
        }
        
        function saveResponse(callback, applyAsComplete) {
            var response = surveyObservable.getResponse();

            response.ApplyAsComplete = applyAsComplete == true;

            surveyObservable.set("isSaving", true);

            $.ajax({
                url: "/survey/update",
                data: kendo.stringify(response),
                success: function (data) {
                    if (data.success) {
                        // Store last-saved version for use in resetting
                        currentSurvey = data.survey;
                        
                        // Update overall status
                        surveyObservable.set("Status", data.survey.Status);
                        
                        // Update score descriptions
                        $.each(data.survey.Pages, function(i, page) {
                            var currentPage = surveyObservable.findPage(page.Id);
                            currentPage.set("Score", page.Score);
                            currentPage.set("ScoreDescription", page.ScoreDescription);
                        });

                        if (callback)
                            callback.call(this, data);

                    }else if (data.validationErrors.length) {
                        processServerValidationErrors(data.validationErrors);
                    }else if (data.message) {
                        alert(data.message);
                    }
                }
            }).always(function() {
                surveyObservable.set("isSaving", false);
            });
        }

        return {
            init: init
        };

    }();


}(this.portal = this.portal || {}, jQuery, _));