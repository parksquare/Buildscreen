angular.module("BuildScreenApp")
    .controller("buildscreencontroller", [
        "$scope", "$timeout", "Build", "DateConvertor", "Isotope", "Search",
        function ($scope, $timeout, Build, DateConvertor, Isotope, Search) {

            $scope.isLoading = true;
            $scope.errorRest = false;
            $scope.builds = [];
            $scope.search = Search;
            Search.builds = $scope.builds;

            $scope.$watch("builds", function (newValue) {
                Search.builds = newValue;
            });

            var pollingTime = 5000,
                pollCounter = 0,
                pollTimeout;

            var added = false;

            var removeBuilds = function (polledBuilds) {
                if (pollCounter % 5 === 0 || $scope.errorRest) {
                    angular.forEach($scope.builds, function (build, keyBuild) {

                        var index = polledBuilds.map(function (el) { return el.id; }).indexOf(build.id);

                        if (index === -1) {
                            $scope.builds.splice(keyBuild, 1);
                        }
                    });
                }
            }

            var addBuilds = function (polledBuild) {
                var index = $scope.builds.map(function (el) { return el.id; }).indexOf(polledBuild.id);

                if (index === -1) {
                    $scope.builds.push(polledBuild);
                    added = true;
                }
            }

            var updateExistingBuilds = function (polledBuild) {
                angular.forEach($scope.builds, function (build, keyBuild) {
                    if (polledBuild.id === build.id) {
                        $scope.builds[keyBuild] = polledBuild;
                    }

                });
            }

            var handleError = function (error) {

                $scope.errorRest = true;

                if (error.status === 0) {
                    $scope.errorMessage = "Unable to connect to API";
                } else {
                    $scope.errorMessage = error.data;
                }


            }

            $scope.poll = function () {
                pollCounter++;
                added = false;
                var since = 2;
                var urlString = "builds";
                if (pollCounter % 5 === 0 || $scope.errorRest) {
                    since = "";
                    urlString = "builds";
                }
                Build.query({ urlString: urlString, since: since }, function (polledBuilds) {

                    $scope.errorRest = false;
                    $scope.errorMessage = "";

                    polledBuilds = DateConvertor.convertDatesToTicks(polledBuilds);
                    removeBuilds(polledBuilds);

                    angular.forEach(polledBuilds, function (polledBuild) {
                        addBuilds(polledBuild);
                        updateExistingBuilds(polledBuild);
                    });
                    $timeout(function () { Isotope.update(added) }, 0);

                    pollTimeout = $timeout($scope.poll, pollingTime);
                }, function (error) {
                    $scope.poll();
                    handleError(error);
                });
            };

            $scope.retrieveBuildsFirstTime = function () {

                Build.query({ urlString: "builds" }, function (builds) {

                    $scope.builds = DateConvertor.convertDatesToTicks(builds);

                    $timeout(function () {
                        Isotope.init();
                        Search.initialize();
                    }, 5);

                    $scope.isLoading = false;
                    $scope.poll();
                }, function (error) {
                    $scope.isLoading = false;
                    handleError(error);

                    $scope.retrieveBuildsFirstTime();
                });
            };

            $scope.retrieveBuildsFirstTime();

            $scope.$on("$destroy", function () {
                $scope.poll = function () { };
                $scope.retrieveBuildsFirstTime = function () { };
                Isotope.update = function () { };
                $timeout.cancel(pollTimeout);
            });

            document.addEventListener("visibilitychange", function () {
                if (document.visibilityState === "visible") {
                    $timeout(function () { Isotope.update(added) }, 0);
                };
            });
        }
    ]);






