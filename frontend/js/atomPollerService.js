angular.module('sklaidaApp')
    .factory('atomPollerService', ['$http', function($http) {
    			return{
    				stop : function(){
    					currentTimeout = null;
    				},
    				start : startPolling
    			}
    			var currentTimeout = null;
                function getFeedLink(links, linkRel) {
                    var res = $.grep(links, function(link) {
                        return link.relation === linkRel;
                    });
                    return res.length ? res[0].uri : null;
                }
                function startPolling(streamUrl, callback) {
                    var nextPageUrl = streamUrl;
                    var readNextPage = readFirstPage;

                    readFirstPage();

                    function readFirstPage() {
                        currentTimeout = null;
                        $http.get(nextPageUrl + '?embed=content', {
                            headers: {
                                'Accept': 'application/vnd.eventstore.atom+json'
                            }
                        })
                        .success(function(data, status, header) {
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
                        .error(function(data, status) {
                            currentTimeout = setTimeout(readNextPage, 1000);
                        });
                    }

                    function readForwardPage() {
                        currentTimeout = null;
                        $http.get(nextPageUrl + "?embed=content", {
                            headers: {
                                'Accept': 'application/vnd.eventstore.atom+json',
                                'ES-LongPoll': 30
                            }
                        })
                        .success(function(data, status, jqXHR) {
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
                        .error(function(data, status) {
                            currentTimeout = setTimeout(readNextPage, 1000);
                        });
                    }
                }
            }]);
