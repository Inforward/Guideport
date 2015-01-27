'use strict';

angular.module('admin.Services').factory('cacheService', [function () {

    function storageSupported() {
        return (typeof (Storage) !== "undefined");
    }

    function parse(value) {
        try {
            return JSON.parse(value);
        } catch (e) {
            return null;
        }
    }

    var sessionCache = storageSupported() ? sessionStorage : {};
    var storageExists = storageSupported();

    return {

        getSessionCache: function(key, defaultValue) {
            if (sessionCache[key])
                return parse(sessionCache[key]);

            return defaultValue;
        },

        setSessionCache: function (key, value) {
            if (value == null)
                return;

            sessionCache[key] = JSON.stringify(value);
        },

        removeSessionCache: function(key) {
            if (sessionCache[key])
                delete sessionCache[key];
        },

        getCache: function (key, defaultValue) {

            if (!storageExists)
                return this.getSessionCache(key, defaultValue);

            var cacheItem = localStorage.getItem(key);

            if (cacheItem == null)
                return defaultValue;

            var item = parse(cacheItem);

            if (item == null)
                return defaultValue;

            return item.value;
        },

        setCache: function(key, value) {
            if (value == null)
                return;

            if (storageExists) {

                var cacheItem = {
                    issued: new Date(),
                    value: value
                };

                localStorage.setItem(key, JSON.stringify(cacheItem));

            } else {
                this.setSessionCache(key, value);
            }
        },

        removeCache: function(key) {
            if (storageExists) {
                localStorage.removeItem(key);
            } else {
                this.removeSessionCache(key);
            }
        }
    }
}]);