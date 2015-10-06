; (function () {
    var propTradeApp = angular.module('propTradeApp', ['ngRoute', 'propTradeServices', 'propTradeDirectives']);

    propTradeApp.config(['$routeProvider', '$httpProvider', function ($routeProvider, $httpProvider) {        
        $routeProvider.
            when('/start', {
                templateUrl: 'ClientApp/views/start.html',
                controller: 'startCtrl'
            }).
            when('/seller', {
                templateUrl: 'ClientApp/views/seller.html',
                controller: 'sellerCtrl'
            }).
            when('/buyer', {
                templateUrl: 'ClientApp/views/buyer.html',
                controller: 'buyerCtrl'
            }).

            when('/about', {
                templateUrl: 'ClientApp/views/about.html'
            }).
            otherwise({
                redirectTo: '/buyer'
            });

        $httpProvider.interceptors.push('authInterceptorService');
    }]);

    propTradeApp.run(['authService', '$rootScope', '$location', function (authService, $rootScope, $location) {
        authService.getAuthData().then(function (data) {
            $rootScope.authData = data;

            if (!$rootScope.authData || !$rootScope.authData.isAuth) {
                $location.path('/start');
                return;
            }

            if ($rootScope.authData.userRole === 'Buyer') {
                $location.path('/buyer');
            } else {
                $location.path('/seller');
            }
        });
    }]);    

    propTradeApp.controller('startCtrl', ['$scope', '$rootScope', '$window', 'authService', function ($scope, $rootScope, $window, authService) {
        $scope.loadings = {
            signingUp: false,
            signingIn: false
        };

        $scope.credentials = {
            Username: '',
            Password: ''
        };

        $scope.newUser = {
            Address: {}
        };

        $scope.login = function () {
            $scope.loadings.signingIn = true;
            authService.login($scope.credentials).then(function (response) {
                $scope.invalidCredentials = false;

                authService.getAuthData().then(function (data) {
                    $rootScope.authData = data;
                    $scope.loadings.signingIn = false;
                    $window.location.reload();
                });
            }, function (err) {
                $scope.invalidCredentials = true;
                $scope.loadings.signingIn = false;
            });
        };

        $scope.register = function () {
            $scope.loadings.signingUp = true;
            authService.register($scope.newUser).then(function (response) {
                authService.getAuthData().then(function (data) {
                    $rootScope.authData = data;
                    $scope.loadings.signingUp = false;;
                    $window.location.reload();
                });
            });
        };
    }]);
    
    propTradeApp.controller('buyerCtrl', ['$scope', '$rootScope', 'propertyService', 'offerService', function ($scope, $rootScope, propertyService, offerService) {
        
        $scope.newOffer = {
            Value: 1,
            Buyer: {
                UserId: ''
            }
        };
        $scope.loadings = {
            properties: false,
            submitting: false
        };

        $scope.loadProperties = function () {
            $scope.loadings.properties = true;
            propertyService.getProperties().then(function (response) {
                $scope.properties = response.data;
                $scope.loadings.properties = false;
            });
        };

        $scope.loadProperties();

        $scope.makeOffer = function (propertyId) {
            $scope.newOffer.Buyer.UserId = $rootScope.authData.userId;
            $scope.newOffer.PropertyId = propertyId;
            offerService.createOffer($scope.newOffer).then(function () {
                $scope.loadProperties();
            });

            // reset offer
            $scope.newOffer = {
                Value: 1,
                Buyer: {
                    UserId: $rootScope.authData.userId
                }
            };
        };
    }]);

    propTradeApp.controller('sellerCtrl', ['$scope', '$rootScope', 'propertyService', function ($scope, $rootScope, propertyService) {
       
        $scope.newProperty = {
            Location: {},
            Owner: {
                UserId: ''
            }
        };
        $scope.loadings = {
            properties: false,
            submitting: false
        };        

        $scope.loadProperties = function () {
            $scope.loadings.properties = true;
            propertyService.getPropertiesForUser($rootScope.authData.userId).then(function (response) {
                $scope.properties = response.data;
                $scope.loadings.properties = false;
            });
        };

        $scope.loadProperties();

        $scope.create = function () {
            $scope.loadings.submitting = true;
            $scope.newProperty.Owner.UserId = $rootScope.authData.userId;
            propertyService.createProperty($scope.newProperty).then(function (response) {
                $scope.loadings.submitting = false;
                $scope.creating = false;
                $scope.loadProperties();
            });
        };

        $scope.delete = function (propertyId) {
            $scope.loadings.properties = true;
            propertyService.deleteProperty(propertyId).then(function (response) {
                $scope.loadProperties();
            });
        };

        $scope.cancel = function () {
            $scope.newProperty = {
                Location: {},
                Owner: {
                    UserId: $rootScope.authData.userId
                }
            };
            $scope.form.$setPristine();
            $scope.form.$setValidity();
            $scope.form.$setUntouched();
        }
    }]);

})();