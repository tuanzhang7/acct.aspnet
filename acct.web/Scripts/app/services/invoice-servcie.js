(function () {
    'use strict';

    var invoiceServiceModule = angular.module('invoiceServices', ['ngResource']);

    invoiceServiceModule.factory('invoiceServices', function ($resource) {
        return $resource('http://localhost:63267/api/invoice?start=:start&limit=:limit', {}, {
            query: { method: 'GET', params: { start: '@start', limit: '@limit' }, isArray: true, headers: { 'auth-token': 'admin 1qazxsw@' } }
        })
    });

    invoiceServiceModule.factory('invoiceServices', function ($resource) {
        return $resource('http://localhost:63267/api/invoice/Customer/:id?start=:page', {}, {
            getByCustomer: { method: 'GET', params: { page: '@page' }, isArray: true, headers: { 'auth-token': 'admin 1qazxsw@' } }
        })
    });

    invoiceServiceModule.factory('invoiceService', function ($resource) {
        return $resource('http://localhost:63267/api/invoice/:id', {}, {
            detail: { method: 'GET', params: { id: '@id' } },
            update: { method: 'PUT', params: { id: '@id' } },
            delete: { method: 'DELETE', params: { id: '@id' } }
        })
    });

})();
