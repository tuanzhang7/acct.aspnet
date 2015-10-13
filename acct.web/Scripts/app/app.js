(function () {
    'use strict';

    var acctApp = angular.module('acctApp', [
        'ngRoute',
        'customerServices',
        'customerController'
    ]);

    phonecatApp.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
          when('/customer', {
              templateUrl: 'customer',
              controller: 'customerController'
          }).
          when('/customer/:id', {
              templateUrl: 'customer/ngDetails',
              controller: 'customerDetailController'
          }).
          otherwise({
              redirectTo: '/customer'
          });
    }]);
})();