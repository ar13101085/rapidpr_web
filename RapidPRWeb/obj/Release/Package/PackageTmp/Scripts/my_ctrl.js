var app = angular.module("my_app", []);

app.controller("my_ctrl", function ($scope, $http, $sce, $interval, $compile) {


    $scope.allbox = [
        'L-1', 'L-2', 'L-3', 'M-1', 'M-2', 'M-3', 'R-1', 'R-2', 'R-3'
    ];

    $scope.mycompiler = function (data) {
        return $sce.trustAsHtml(data);
    }

    $scope.AllVideo = [];
    $scope.isLoadAllVideo = false;
    $scope.getAllVideo = function () {
        if (!$scope.isLoadAllVideo) {
            $http({
                method: "get",
                url: "/Home/GetAllVideo",
                data: "{}"
            }).then(function (response) {
                $scope.AllVideo = response.data;
            });
        }
        $scope.isLoadAllVideo = true;
    }



    $scope.AllAdvertise = [];
    $scope.isLoadAllAdvertise = false;
    $scope.getAllAdvertise = function () {
        if (!$scope.isLoadAllAdvertise) {
            $http({
                method: "get",
                url: "/Home/GetAllAdvertise",
                data: "{}"
            }).then(function (response) {
                $scope.AllAdvertise = response.data;
            });
        }
        $scope.isLoadAllAdvertise = true;
    }


    $scope.publishAdvertise = function (ix) {
        var data = {
            'id':$scope.AllAdvertise[ix].AdvertiseId,
            'boxName':$scope.AllAdvertise[ix].BoxName,
            'duration': $scope.AllAdvertise[ix].LiveDurationInSec
        };
        $http({
            method: "post",
            url: "/home/AdvertisePublish",
            data: data
        }).then(function (response) {
            if (response.data.res === true) {
                $scope.AllAdvertise[ix].AdvertiseId = response.data.advertise.AdvertiseId;
                $scope.AllAdvertise[ix].AdvertiseUploadTime = response.data.advertise.AdvertiseUploadTime;
                $scope.AllAdvertise[ix].AdvertiseName = response.data.advertise.AdvertiseName;
                $scope.AllAdvertise[ix].LiveDurationInSec = response.data.advertise.LiveDurationInSec;
                $scope.AllAdvertise[ix].BoxName = response.data.advertise.BoxName;
                $scope.AllAdvertise[ix].IsPublish = response.data.advertise.IsPublish;
            }
        });

    }


    $scope.AllText = [];
    $scope.isLoadAllText = false;
    $scope.getAllText = function () {
        if (!$scope.isLoadAllText) {
            $http({
                method: "get",
                url: "/Home/GetAllText",
                data: "{}"
            }).then(function (response) {
                $scope.AllText = response.data;
            });
        }
        $scope.isLoadAllText = true;
    }




    $scope.addmeerror = function (msg) {
        $scope.errormsg = msg;
        $("#modalerror").modal("show");
    };
    $scope.closemeerror = function () {
        $scope.errormsg = "Something is missing";
        $("#modalerror").modal("hide");
    };
    $scope.addmesuccess = function (msg) {
        $scope.successmsg = msg;
        $("#modalsuccess").modal("show");
    };
    $scope.closemesuccess = function () {
        $scope.errormsg = "Successfully Complete...";
        $("#modalsuccess").modal("hide");
    };
    $scope.addmeconfirm = function (msg) {
        $scope.confirmmsg = msg;
        $("#modalconfirm").modal("show");
    };
    $scope.closemeconfirm = function () {
        $scope.confirmmsg = "Confirm this action";
        $("#modalconfirm").modal("hide");
    };

});