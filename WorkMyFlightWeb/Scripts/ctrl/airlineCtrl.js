module.controller("airlineCtrl", AirlineCtrl);
// airline ctrl
function AirlineCtrl($scope, $rootScope) {
    $scope.airline = {};
    $scope.SubmitAirline = (airline) => {

        // If Not same return False.     
        if (airline.password != airline.confPwd) {
            return swal.fire('Password Was Not Confirmed', 'Please Confirm Password')
        }
        // If same return True and proceed to ajax sending all the airline info
        else 
        $.ajax({
            url: "/api/AnonymousFacade/AddNewAirlineToRedis",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(airline)
        }).then((result) => {
            if (result == true);
            console.log('ok');
            return swal.fire('THANKS FOR SIGINING', 'will send you an email for confirmation   :        '+'(user name: ' + $scope.airline.userName + ', email address: ' + $scope.airline.email + ')');
            })
                .catch ((err) => {
                    console.log(err)
                    swal.fire($scope.airline.airlineName + '  User Already Exists' , 'Try Different User Name')
                })
    }
}
// open airline registration form func    
function openFormAirlline() {
    document.getElementById("myForm2").style.display = "block";
    document.getElementById("login").style.display = "none";
}
// close airline registration form func 
function closeFormAirline() {
    document.getElementById("myForm2").style.display = "none";
}
// close login form func 
function closeForm() {
    document.getElementById("login").style.display = "none";
}