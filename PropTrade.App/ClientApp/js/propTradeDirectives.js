; (function () {
    var propTradeDirectives = angular.module('propTradeDirectives', []);

    propTradeDirectives.directive('loginLogout', ['$rootScope', function ($rootScope) {
        return {
            controller: ['$scope', 'authService', '$location', function ($scope, authService, $location) {
                
                $scope.logout = function () {
                    authService.logout();
                    $location.path('/start');
                };
            }],
            templateUrl: 'ClientApp/views/templates/loginLogout.html'
        };
    }]);

})();