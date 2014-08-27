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
                pollForResults: function(searchResultUrlToPoll, callback) {
                    console.info('Transformation magic happening here for now. The returned location is : ', searchResultUrlToPoll);
                    var locationToPoll = searchResultUrlToPoll.replace('-', '').replace('/', '-');
                    var locationToPoll = 'http://localhost:2113/streams/' + locationToPoll;
                    var currentTimeout = null;
                    startPolling(currentTimeout, locationToPoll, callback);
                }
            }

            function getFeedLink(links, linkRel) {
                var res = $.grep(links, function(link) {
                    return link.relation === linkRel;
                });
                return res.length ? res[0].uri : null;
            }

            function startPolling(currentTimeout, streamUrl, callback) {
                var nextPageUrl = streamUrl;
                var readNextPage = readFirstPage;

                readFirstPage();

                function readFirstPage() {
                    currentTimeout = null;
                    $.ajax(nextPageUrl + "?embed=content", {
                        dataType: 'json',
                        headers: {
                            'Accept': 'application/vnd.eventstore.atom+json'
                        }
                    })
                    .done(function(data, textStatus, jqXHR) {
                        var lastLink = getFeedLink(data.links, 'last');
                        if (!lastLink) {
                            // head is the last page already
                            if (data.entries) {
                                for (var i = 0, n = data.entries.length; i < n; i += 1) {
                                    var event = data.entries[n - i - 1].content;
                                    if (event)
                                        callback(null, event);
                                }
                            }
                            nextPageUrl = getFeedLink(data.links, 'previous');
                        } else {
                            nextPageUrl = lastLink;
                        }
                        readNextPage = readForwardPage;
                        currentTimeout = setTimeout(readNextPage, 0);
                    })
                    .fail(function(jqXHR, textStatus, errorThrown) {
                        currentTimeout = setTimeout(readNextPage, 1000);
                    });
                }

                function readForwardPage() {
                    currentTimeout = null;
                    $.ajax(nextPageUrl + "?embed=content", {
                        dataType: 'json',
                        headers: {
                            'Accept': 'application/vnd.eventstore.atom+json',
                            'ES-LongPoll': 30
                        }
                    })
                    .done(function(data, textStatus, jqXHR) {
                        if (data.entries) {
                            for (var i = 0, n = data.entries.length; i < n; i += 1) {
                                var event = data.entries[n - i - 1].content;
                                if (event)
                                    callback(null, event);
                            }
                        }
                        var prevLink = getFeedLink(data.links, 'previous');
                        nextPageUrl = prevLink || nextPageUrl;
                        currentTimeout = setTimeout(readNextPage, prevLink ? 0 : 1000);
                    })
                    .fail(function(jqXHR, textStatus, errorThrown) {
                        currentTimeout = setTimeout(readNextPage, 1000);
                    });
                }
            }
        }
    ]);
