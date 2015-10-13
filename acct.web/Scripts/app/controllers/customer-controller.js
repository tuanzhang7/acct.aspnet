(function () {
    'use strict';

    var myApp = angular.module('acctApp');


    myApp.controller('customerController', customerController);

    customerController.$inject = ['$scope', 'customerServices'];

    function customerController($scope, customerServices) {
        $scope.customers = customerServices.query({ start: 0, limit: 10 });
    }


    myApp.controller('customerDetailController', customerDetailController);

    customerDetailController.$inject = ['$scope', 'customerService'];

    function customerDetailController($scope, customerService) {
        $scope.customer = customerService.detail({ id: 4 });
    }
 
})();




