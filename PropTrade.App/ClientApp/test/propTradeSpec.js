describe('propTradeApp', function () {
    var $rootScope,
        $scope,
        $httpBackend,
        $controller/*,
        calculator*/;

    beforeEach(angular.mock.module('propTradeApp'));

    beforeEach(inject(function ($injector) {
        $rootScope = $injector.get('$rootScope');
        $scope = $rootScope.$new();
        $httpBackend = $injector.get('$httpBackend');
        $controller = $injector.get('$controller');
        //calculator = $injector.get('calculator');
    }));

    describe('propTradeCtrl', function () {

        beforeEach(function () {
            controller = function () {
                $controller('propTradeCtrl', {
                    '$rootScope': $rootScope,
                    '$scope': $scope
                });
            };
        });

        it('properly initializes the view model', function () {
            controller();

            expect(true).toBeTruthy();
        });        

    });
    // END: propTradeCtrl tests
    
});