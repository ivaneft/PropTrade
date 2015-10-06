describe('propTradeApp', function () {
    var $rootScope,
        $scope,
        $httpBackend,
        $controller/*,
        propTradeServices*/;

    beforeEach(angular.mock.module('propTradeApp'));

    beforeEach(inject(function ($injector) {
        $rootScope = $injector.get('$rootScope');
        $scope = $rootScope.$new();
        $httpBackend = $injector.get('$httpBackend');
        $controller = $injector.get('$controller');
        //propTradeServices = $injector.get('propTradeServices');
    }));

    describe('startCtrl', function () {

        beforeEach(function () {
            controller = function () {
                $controller('startCtrl', {
                    '$rootScope': $rootScope,
                    '$scope': $scope
                });
            };
        });

        it('properly initializes the startCtrl', function () {
            controller();

            var expectedLoadings = {
                signingUp: false,
                signingIn: false
            };

            var expectedCredentials = {
                Username: '',
                Password: ''
            };

            var expectedNewUser = {
                Address: {}
            };

            expect($scope.loadings).toEqual(expectedLoadings);
            expect($scope.credentials).toEqual(expectedCredentials);
            expect($scope.newUser).toEqual(expectedNewUser);
        });        

    });
    // END: startCtrl tests
    
});