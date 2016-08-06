var myApp = angular.module("myApp", []);

myApp.controller("RegisterCon",
    function ($scope, $http) {
        $scope.name = "Alui";
        $scope.AddUser = function() {
            $http.post("/Register/Add", { PersonName: $scope.PersonName })
            .success(function (result) {
                $scope.result = result;
            })
            .error(function (data) {
                console.log(data);
            });
        }
});