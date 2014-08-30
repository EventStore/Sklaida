angular.module('sklaidaApp', []);
angular.module('sklaidaApp')
.filter('range', function() {
    return function(input, total) {
        total = parseInt(total);
        for (var i=0; i < total; ++i) {
            input.push(i);
        }
        return input;
    };
});