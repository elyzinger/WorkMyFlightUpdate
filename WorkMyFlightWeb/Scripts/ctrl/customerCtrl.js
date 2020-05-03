module.controller("customerCtrl", CustomerCtrl);
// customer ctrl
function CustomerCtrl($scope, $rootScope) {
    $scope.customer = {};
    $rootScope.login = {};
    // submit customer func
    $scope.SubmitCustomer = (customer) => {

        // If Not same return False.     
        if (customer.password != customer.confPwd) {
            return swal.fire('Password Was Not Confirmed', 'Please Confirm Password')
        }
        // If same return True and proceed to ajax sending customer info 
        else 

            $.ajax({
                url: "/api/AnonymousFacade/AddNewCustomerToRedis",
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(customer)
            }).then((result) => {
                if (result == true) {
                    console.log('ok');
                    return swal.fire('THANKS FOR SIGINING', 'will send you an email for confirmation   :        ' + '(user name: ' + $scope.customer.userName + ', email address: ' + $scope.customer.email + ')');
                }
                })
                .catch((err) => { console.log(err) 
                    swal.fire('User Allready Exists', 'Try Different User Name')
                })

    }
    // open login func
    $rootScope.openFormLogin = () => {
        document.getElementById("login").style.display = "block";
        document.getElementById("customerForm").style.display = "none";
        document.getElementById("myForm2").style.display = "none";
    }
    // submit login func sending user name and password by ajax
    $rootScope.SubmitLogin = () => {

        $.ajax({
            url: "/api/AnonymousFacade/WriteHello",
            type: 'GET',
            contentType: 'application/json',
            headers: {
                'Authorization': 'Basic ' + btoa($scope.login.username + ':' + $scope.login.pwd)
            },         
        }).then((result) => {          
            console.log('ok');
            console.log(result);
                const user = btoa($scope.login.username + ':' + $scope.login.pwd);
                jsonUser = JSON.stringify(user);
                localStorage.setItem('user', jsonUser);
              
        })
            .catch((err) => {
                console.log(err)
                swal.fire('Wrong Password Or Username', 'Check Yourself')
            })
        
    }
}


function closeForm() {
    document.getElementById("login").style.display = "none";
}

function openFormCustomer() {
    document.getElementById("customerForm").style.display = "block";
    document.getElementById("login").style.display = "none";
}

function closeFormCustomer() {
    document.getElementById("customerForm").style.display = "none";
}