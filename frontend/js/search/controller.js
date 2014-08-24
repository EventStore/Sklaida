angular.module('sklaidaApp')
    .controller('searchController', ['$scope', 'searchService',
        function SearchController($scope, searchService) {
            $scope.results = [];
            $scope.init = function() {
                $scope.fieldsToSubmit = {
                    SearchType: "Template",
                    DesiredDeliveryDate: formatDate(new Date())
                }
            }
            $scope.submit = function() {
                searchService.search($scope.fieldsToSubmit, searchCompleted);
            }

            function searchCompleted(err, response) {
                searchService.pollForSearchResult(response.Location, pollResultsReturned);
            }

            function pollResultsReturned(err, results) {
                $scope.results = [];
                results.entries.forEach(function(entry) {
                    if (entry.isJson) {
                        $scope.results.push({
                            data: JSON.parse(entry.data),
                            type: entry.eventType
                        });
                    }
                });
            }

            function formatDate(dateToFormat) {
                var date = dateToFormat.getDate();
                var month = dateToFormat.getMonth() + 1;
                var year = dateToFormat.getFullYear();
                return date + "/" + month + "/" + year;
            }
        }
    ]);
