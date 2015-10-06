; (function () {
    var propTradeServicesApp = angular.module('propTradeServices', []);

    propTradeServicesApp.factory('propertyService', ['$http', function ($http) {
        var url = '/api/v1/properties';
        return {
            getProperties: function () {
                return $http.get(url);
            },
            getPropertiesForUser: function (userId) {
                return $http.get(url + '?userId=' + userId);
            },
            getProperty: function (id) {
                if (!property.Id) {
                    throw 'Id cannot be empty';
                }

                return $http.get(url + '/' + id);
            },
            createProperty: function (property) {
                return $http.post(url, property);
            },
            updateProperty: function (property) {
                if (!property.Id) {
                    throw 'Id cannot be empty';
                }

                return $http.put(url + '/' + property.Id, property);
            },
            deleteProperty: function (id) {
                return $http.delete(url + '/' + id);
            }
        };
    }]);

    propTradeServicesApp.factory('offerService', ['$http', function ($http) {
        var url = '/api/v1/offers';
        return {
            getOffers: function () {
                return $http.get(url);
            },
            getOffer: function (id) {
                return $http.get(url + '/' + id);
            },
            createOffer: function (offer) {
                return $http.post(url, offer);
            },
            updateOffer: function (offer) {
                if (!offer.Id) {
                    throw 'Id cannot be empty';
                }

                return $http.put(url + '/' + offer.Id, offer);
            },
            deleteOffer: function (id) {
                return $http.delete(url + '/' + id);
            }
        };
    }]);

    propTradeServicesApp.factory('authService', ['$http', '$q', '$document', '$location', function ($http, $q, $document, $location) {

        var authServiceFactory = {};

        var _authData = {
            isAuth: false,
            userName: '',
            userId: '',
            userDisplayName: '',
            userRole: 'Buyer'
        };

        var _register = function (newUser) {
            _logOut();
            var deferred = $q.defer();

            $http.post('/api/v1/users', newUser).success(function (response) {
                deferred.resolve(response);
                _login({ Username: newUser.Username, Password: newUser.Password });
            }).error(function (err, status) {
                deferred.reject(err);
            });

            return deferred.promise;

        };

        var _login = function (credentials) {

            var deferred = $q.defer();

            $http.get('/api/v1/authenticate?username=' + credentials.Username + '&password=' + credentials.Password)
                .success(function (response) {

                    _authData.isAuth = true;
                    _authData.userName = credentials.Username;
                    var now = new Date();
                    $document.prop('cookie', 'propTradeAuth=' + response + '; expires=' + (now.getMinutes() + 30*60000) + '; path=/');

                    deferred.resolve(response);

                }).error(function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });

            return deferred.promise;
        };

        var _logOut = function () {

            $document.prop('cookie', 'propTradeAuth=;expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/');

            authenticationData = {
                isAuth: false,
                userName: '',
                userId: '',
                userDisplayName: '',
                userRole: 'Buyer'
            };

        };

        var _getAuthData = function () {
            var authToken = getCookie('propTradeAuth');
            var deferred = $q.defer();

            if (!authToken) {
                _logOut();
                deferred.resolve(_authData);
                $location.path('/start');
            } else {
                $http.get('/api/v1/identity')
                .success(function (response) {

                    _authData = {
                        isAuth: true,
                        userName: response.Username,
                        userId: response.Id,
                        userDisplayName: response.FirstName + ' ' + response.LastName,
                        userRole: response.CurrentTradeRole
                    };

                    deferred.resolve(_authData);

                }).error(function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });
            }

            return deferred.promise;
        };

        authServiceFactory.register = _register;
        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.getAuthData = _getAuthData;

        return authServiceFactory;
    }]);

    propTradeServicesApp.factory('authInterceptorService', ['$q', '$location', '$document', function ($q, $location, $document) {

        var authInterceptorServiceFactory = {};

        var _request = function (config) {

            config.headers = config.headers || {};

            var authToken = getCookie('propTradeAuth');
            if (authToken) {
                config.headers.Authorization = 'Basic ' + authToken;
            }

            return config;
        }

        var _responseError = function (rejection) {
            if (rejection.status === 401) {
                $location.path('/start');
            }
            return $q.reject(rejection);
        }

        authInterceptorServiceFactory.request = _request;
        authInterceptorServiceFactory.responseError = _responseError;

        return authInterceptorServiceFactory;
    }]);

    var getCookie = function (name) {
        var value = "; " + document.cookie;
        var parts = value.split("; " + name + "=");
        if (parts.length == 2) return parts.pop().split(";").shift();
    }
})();