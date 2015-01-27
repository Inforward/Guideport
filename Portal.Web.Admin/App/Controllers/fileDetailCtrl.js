'use strict';

angular.module('admin.Controllers')
    .controller("fileDetailCtrl", ['$scope', '$routeParams', '$modal', '$timeout', 'contentService', 'userService', 'affiliateService', function ($scope, $routeParams, $modal, $timeout, contentService, userService, affiliateService) {

        $scope.profileTypes = [];
        $scope.affiliates = [];
        $scope.uploader = {
            visible: true
        };

        $scope.content = {
            SiteID: parseInt($routeParams.siteId),
            SiteContentID: $routeParams.id ? parseInt($routeParams.id) : null,
            SiteContentTypeID: 3, // file
            Permalink: '',
            Title: '',
            PublishDateUtc: new Date(),
            IncludeInKnowledgeLibrary: false
        };

        $scope.siteChanged = function () {
            $scope.setSite();
            $scope.setPermalink();
            $scope.refreshTopics();
        };

        $scope.setSite = function () {
            $scope.content.Site = _.find($scope.sites, function (s) { return s.SiteID == $scope.content.SiteID; });
        };

        $scope.isNew = function () {
            return $scope.content.SiteContentID == null;
        };

        $scope.refreshTopics = function () {
            
            contentService.getTopics($scope.content.SiteID).then(function(data) {
                $scope.topics = data.Topics;
                $scope.subTopics = data.Subtopics;
                topicsDataSource.read();
                subTopicsDataSource.read();
            });
        };

        $scope.updateContent = function (content) {
            $scope.content = content;
            $scope.content.IncludeInKnowledgeLibrary = content.KnowledgeLibraries.length > 0;
            $scope.title = $scope.content.Title;
            $scope.uploader.visible = !(content.FileID > 0);

            if (content.KnowledgeLibraries.length)
                content.KnowledgeLibrary = content.KnowledgeLibraries[0];

            if ($scope.content.PublishDateUtc)
                $scope.content.PublishDateUtc = new Date($scope.content.PublishDateUtc);
        };

        $scope.saveFile = function (form) {
            var file = $scope.content;
            
            if (file.IncludeInKnowledgeLibrary) {
                file.KnowledgeLibraries = [file.KnowledgeLibrary];
            } else {
                delete file.KnowledgeLibraries;
            }

            $scope.isSaving = true;

            contentService.saveContent(file).then(function (response) {
                $scope.updateContent(response);
                $scope.isSaving = false;
                form.$setPristine();
            });
        };

        $scope.uploadOptions = {
            async: { saveUrl: '/api/contents/files/upload', autoUpload: true },
            localization: { select: 'Select file...', uploadSelectedFiles: 'Upload File' },
            enabled: true,
            multiple: false,
            success: function (e) {
                $scope.updateContent(e.response);
                $scope.$digest();
                $(".k-upload-files").remove();
                $(".k-upload-status").remove();
            },
            upload: function (e) {
                var file = $scope.content;

                if (!file.Title.length) {
                    alert('You must enter a Title for this file before you can upload.');
                    e.preventDefault();
                    return;
                }

                e.data = file;
            }
        };

        $scope.setPermalink = function () {
            if (!$scope.content.Title.length) {
                if ($scope.isNew()) $scope.content.Permalink = '';
                return;
            }

            if ($scope.isNew()) {
                contentService.generatePermalink($scope.content).then(function (response) {
                    $scope.content.Permalink = response.Permalink;
                });
            }
        };


        var topicsDataSource = new kendo.data.DataSource({
            transport: { read: function(e) { e.success($scope.topics); } }
        });

        var subTopicsDataSource = new kendo.data.DataSource({
            transport: { read: function (e) { e.success($scope.subTopics); } }
        });


        //
        // Initialize
        //
        contentService.getTopics($scope.content.SiteID).then(function (data) {
            $scope.topics = data.Topics;
            $scope.subTopics = data.Subtopics;

            $scope.topicOptions = {
                optionLabel: { Text: 'Select a Measure', Topic: '' },
                dataTextField: 'Text',
                dataValueField: 'Topic',
                valuePrimitive: true,
                dataSource: topicsDataSource
            };

            $scope.subTopicOptions = {
                optionLabel: { Subtopic: 'Select a Subtopic', Topic: '' },
                dataTextField: 'Subtopic',
                dataValueField: 'Subtopic',
                valuePrimitive: true,
                cascadeFrom: 'Topic',
                dataSource: subTopicsDataSource
            };
        });

        userService.getProfileTypes().then(function (data) {
            $scope.profileTypes = data;
        });

        affiliateService.getAffiliates().then(function (data) {
            $scope.affiliates = data;
        });

        if (!$scope.isNew()) {
            contentService.getContent($scope.content.SiteContentID).then(function (content) {
                $scope.updateContent(content);
                $scope.refreshTopics();
            });
        } else {
            contentService.getSites().then(function (data) {
                $scope.sites = data;
                $scope.setSite();
                $scope.refreshTopics();
                $scope.title = 'New File';
            });
        }

    }]);