﻿angular.module('app', ['ngRoute', 'adminTools'])
	.config(function($routeProvider) {
		$routeProvider
			.when('/admin/componentInterfaces', {
				templateUrl: '/ComponentInterface/Index',
				controller: 'componentInterfaceManagementController'
			})
			.when('/welcome', {
				templateUrl: '/Home/Welcome'
			})
			.otherwise({
				redirectTo: 'welcome'
			});
	});