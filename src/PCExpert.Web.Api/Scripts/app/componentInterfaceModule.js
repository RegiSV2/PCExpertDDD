angular.module('adminTools', ['ngResource'])
	.factory('compIntResource', [
		function($resource) {
			return $resource('api/ComponentInterface/:Id', { Id: "@Id" });
		}
	])
	.controller('componentInterfaceManagementController', [
		'compIntRepository', function(compIntResource) {
			console.log('in controller');

			var allInterfaces = compIntResource.query();
			console.log(allInterfaces);
		}
	]);