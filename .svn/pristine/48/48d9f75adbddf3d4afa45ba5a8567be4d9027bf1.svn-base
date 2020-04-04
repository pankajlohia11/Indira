var app1 = angular.module("AngularApp", []);

app1.filter('jsonDate', ['$filter', function ($filter)
{
    return function (input, format) {
        return (input)
            ? $filter('date')(parseInt(input.substr(6)), format)
            : '';
    };
}]);

app1.filter('customNumberCulture', function ($filter)
{
    return function (input)
    {
        var value = input.toLocaleString(
            'de-DE', // leave undefined to use the browser's locale,
            // or use a string like 'en-US' to override it.
            { minimumFractionDigits: 2 }
        );
        return value;
    };
}
);