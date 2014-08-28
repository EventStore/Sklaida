angular.module('sklaidaApp')
    .factory('searchService', ['$http', 'atomPollerService',
        function searchService($http, atomPollerService) {
            return {
                search: function(data, callback) {
                    var headers = {
                        'Content-Type': 'application/vnd.ouroinc.searchrequest+json; charset=utf-8'
                    };
                    $http.post('http://localhost:9000/search', data, {
                        headers: headers
                    })
                    .success(function(data, status, header, config) {
                        callback(null, {
                            Location: header('Location')
                        });
                    });
                },
                pollForResults: function(searchResultUrlToPoll, callback) {
                    console.info('Transformation magic happening here for now. The returned location is : ', searchResultUrlToPoll);
                    var locationToPoll = searchResultUrlToPoll.replace('-', '').replace('/', '-');
                    var locationToPoll = 'http://localhost:2113/streams/' + locationToPoll;
                    var currentTimeout = null;
                    atomPollerService.start(locationToPoll, callback);
                    // startPolling($http, currentTimeout, locationToPoll, callback);
                }
            }
        }
    ]);
