(function () {
    'use strict';

    var customerServiceModule = angular.module('customerServices', ['ngResource']);

    customerServiceModule.factory('customerServices', function ($resource) {
        return $resource('http://localhost:63267/api/Customer?start=:start&limit=:limit', {}, {
            query: { method: 'GET', params: { start: '@start', limit: '@limit' }, isArray: true, headers: { 'auth-token': 'admin 1qazxsw@' } }
        })
    });


    customerServiceModule.factory('customerService', function ($resource) {
        return $resource('http://localhost:63267/api/Customer/:id', {}, {
            detail: { method: 'GET', params: { id: '@id' } },
            update: { method: 'PUT', params: { id: '@id' } },
            delete: { method: 'DELETE', params: { id: '@id' } }
        })
    });

})();
//(function () {
//    'use strict';

//    angular
//        .module('acctApp',[])
//        .service('customerService', service);

//    service.$inject = ['$resource'];

//    function service($resource) {
//        this.getData = getData;

//        function getData() {
//            //var customers = [
//            //{ Id: 6, Name: '2ADVANCED PTE LTD', ContactName: "Mr Freddy Khong", Phone: "6552 1259" },
//            //{ Id: 7, Name: '3A-COOL SERVICES PTE LTD', ContactName: "Eddie Gan", Phone: "6552 1259" },
//            //{ Id: 8, Name: '5 FOOT WAY INN', ContactName: "Mr Wei Hao", Phone: "6281 0306" },
//            //{ Id: 9, Name: 'A SENSOR TECH PTE LTD', ContactName: "Mr Watson / Saiful", Phone: "6483 5702" },
//            //{ Id: 10, Name: 'A PLUS FOOD PLACE', ContactName: "Mr.Chye Joon Num", Phone: "6223 8083" }];
//            //return customers;
//            return $resource('http://localhost:63267/api/Customer?start=0&limit=10', {}, {
//                query: { method: 'GET', params: {}, isArray: true }
//            });
//            //return $http('http://localhost:63267/api/Customer?start=0&limit=10');
//        }
//    }
//})();


