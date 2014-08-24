angular.module('sklaidaApp')
    .factory('searchService', ['$http',
        function searchService($http) {
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
                pollForSearchResult: function(searchResultUrlToPoll, callback) {
                    console.info('weird transformation magic happening here for now');
                    console.info('the returned location is : ', searchResultUrlToPoll);
                    var locationToPoll = searchResultUrlToPoll.replace('-', '').replace('/', '-');
                    var locationToPoll = 'http://localhost:2113/streams/' + locationToPoll + "?embed=tryharder";

                    console.info('transformed to : ', locationToPoll);
                    var headers = {
                        'Accept': 'application/vnd.eventstore.atom+json'
                    };

                    var schedule = function() {
                        setTimeout(function() {
                            $http.get(locationToPoll, {
                                headers: headers
                            })
                                .success(function(data, status, header, config) {
                                    callback(null, data);
                                    schedule();
                                });
                        }, 1000);
                    }

                    schedule();
                }
            }
        }
    ]);
