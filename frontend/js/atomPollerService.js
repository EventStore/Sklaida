angular.module('sklaidaApp')
    .factory('atomPollerService', ['$http', '$q', '$timeout',
        function($http, $q, $timeout) {
            var pollingInterval = 1000;
            var currentPoller = null;
            var timer = null;
            return {
                stop: function() {
                    if (currentPoller !== null) {
                        currentPoller.stop();
                    }
                },
                start: function(urlToPoll, callback) {
                    currentPoller = atomPoller($http, $timeout, $q, urlToPoll, callback);
                }
            }

            function atomPoller($http, $timeout, $q, streamUrl, callback) {
                var nextPageUrl = streamUrl;
                var readNextPage = readFirstPage;
                var requestCanceller = $q.defer();

                readFirstPage();

                function readFirstPage() {
                    $http.get(nextPageUrl + '?embed=content', {
                        headers: {
                            'Accept': 'application/vnd.eventstore.atom+json'
                        },
                        timeout: requestCanceller.promise
                    })
                    .success(function(data) {
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
                        timer = $timeout(readNextPage, 0);
                    })
                    .error(function(data, statuscode){
                        console.info('error');
                        console.info('ensuring that the request is cancelled');
                        console.log(timer);
                        console.log(requestCanceller);
                        timer = $timeout(readNextPage, pollingInterval);
                    });
                }

                function readForwardPage() {
                    $timeout.cancel(timer);
                    $http.get(nextPageUrl + "?embed=content", {
                        headers: {
                            'Accept': 'application/vnd.eventstore.atom+json',
                            'ES-LongPoll': 30
                        },
                        timeout: requestCanceller.promise
                    })
                    .success(function(data) {
                        if (data.entries) {
                            for (var i = 0, n = data.entries.length; i < n; i += 1) {
                                var event = data.entries[n - i - 1].content;
                                if (event)
                                    callback(null, event);
                            }
                        }
                        var prevLink = getFeedLink(data.links, 'previous');
                        nextPageUrl = prevLink || nextPageUrl;
                        timer = $timeout(readNextPage, prevLink ? 0 : pollingInterval);
                    })
                    .error(function(data, statuscode){
                        timer = $timeout(readNextPage, pollingInterval);
                    });
                }
                return {
                    stop: function() {
                        if (timer) $timeout.cancel(timer);
                        if (requestCanceller) requestCanceller.resolve('cancelled');
                    }
                }
            }

            function getFeedLink(links, linkRel) {
                var res = $.grep(links, function(link) {
                    return link.relation === linkRel;
                });
                return res.length ? res[0].uri : null;
            }
        }
    ]);
