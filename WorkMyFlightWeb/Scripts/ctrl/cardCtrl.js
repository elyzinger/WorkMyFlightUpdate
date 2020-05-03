module.controller("cardCtrl", CardCtrl)

// cards ctrl 
function CardCtrl($scope, $http, globalConst, apiService, valueService) {
    
    apiService.GetCards()
    $scope.apiResult = valueService.apiResult;
    //$scope.date = Date.now.sqlDateStr();
    //console.log($scope.date);
    // open promotion flight form
    $scope.PickFlight = () => {
      
        document.getElementById("cardPaymentStyle").style.display = "block";
    }
    // close promotion flight form
    $scope.Back = () => {
        
        document.getElementById("cardPaymentStyle").style.display = "none";
    }
   // but ticket func *not ready yet
    $scope.BuyTicket = () => {
        swal.fire('Enjoy Your Flight');
   
    }
}


