var app = angular.module('DGDAapp', []);

// Override contenteditable to allow two-way binding
app.directive('contenteditable', function () {
    return {
        restrict: 'A', // only activate on element attribute
        require: '?ngModel', // get a hold of NgModelController
        link: function (scope, element, attrs, ngModel) {
            if (!ngModel) return; // do nothing if no ng-model

            // Specify how UI should be updated
            ngModel.$render = function () {
                element.html(ngModel.$viewValue || '');
            };

            // Listen for change events to enable binding
            element.on('blur keyup change', function () {
                scope.$apply(readViewText);
            });

            // Write data to the model
            function readViewText() {
                var html = element.html();
                // When we clear the content editable the browser leaves a <br> behind
                // If strip-br attribute is provided then we strip this out
                if (attrs.stripBr && html == '<br>') {
                    html = '';
                }
                ngModel.$setViewValue(html);
            }
        }
    };
});

// Main controller for reading, creating, updating and deleting products.
function ProductsController($scope, $http) {
    $http.get('http://localhost:1226/REST/ReadProducts.aspx').
    success(function(data) {
        $scope.Products = data.Products;
    }).
    error(function(data, status, headers, config) {
        $scope.errorMessage = "Could not read products from database.";
    });
    
    $scope.updateProduct = function(productID, productName, productDescription, productPrice, productInStock) {
        var req = {
            method: 'POST',
            url: 'http://localhost:1226/REST/UpdateProduct.aspx',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            // Transform request to simplify handling of post data on REST server
            transformRequest: function(obj) {
                var str = [];
                for(var p in obj)
                    str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                return str.join("&");
            },
            data: {JSON: "{\"ID\":\"" + productID +
                         "\", \"Name\":\"" + productName +
                         "\", \"Description\":\"" + productDescription + 
                         "\", \"Price\":\"" + productPrice +
                         "\", \"InStock\":\"" + productInStock + "\"}"},
        }
        
        $http(req).
        success(function(data, status, headers, config) {
            $scope.message = "Product was updated successfully!";
        }).
        error(function(data, status, headers, config) {
            $scope.errorMessage = "Could not update product.";
        });
    };
    
    $scope.createProduct = function(productName, productDescription, productPrice, productInStock) {
        var req = {
            method: 'POST',
            url: 'http://localhost:1226/REST/CreateProduct.aspx',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            // Transform request to simplify handling of post data on REST server
            transformRequest: function(obj) {
                var str = [];
                for(var p in obj)
                    str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                return str.join("&");
            },
            data: {JSON: "{\"Name\":\"" + productName +
                         "\", \"Description\":\"" + productDescription + 
                         "\", \"Price\":\"" + productPrice +
                         "\", \"InStock\":\"" + productInStock + "\"}"},
        }
        
        $http(req).
        success(function(data, status, headers, config) {
            $scope.message = "Product was created successfully!";
            $scope.$apply();
            $scope.Products.push({
                ID: data.ProductID,
                Name: productName,
                Description: productDescription,
                Price: productPrice,
                InStock: productInStock
            });
        }).
        error(function(data, status, headers, config) {
            $scope.errorMessage = "Could not create product.";
        });
    };

    $scope.deleteProduct = function(productID) {
        var req = {
            method: 'POST',
            url: 'http://localhost:1226/REST/DeleteProduct.aspx',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            // Transform request to simplify handling of post data on REST server
            transformRequest: function(obj) {
                var str = [];
                for(var p in obj)
                    str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                return str.join("&");
            },
            data: {JSON: "{\"ID\":\"" + productID + "\"}"},
        }
        
        $http(req).
        success(function(data, status, headers, config) {
            $scope.message = "Product was delete successfully!";
            var index = $scope.Products.indexOf(productID)
            $scope.Products.splice(index, 1);
            $scope.message = "Product was deleted successfully!";
        }).
        error(function(data, status, headers, config) {
            $scope.errorMessage = "Could not delete product.";
        });
    };
}