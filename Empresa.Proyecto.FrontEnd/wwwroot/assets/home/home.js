



document.addEventListener("DOMContentLoaded", function (event) {


    document.getElementById("btnEnviar").addEventListener('click', function () {

        LoadingShow();

        postData('Home/GetMinion', { nombre: document.getElementById("txtNombre").value })
            .then((data) => {
                document.getElementById("idResult").innerHTML = data;
                LoadingHide();
            }).catch((error) => {
                console.log(error)
            });

    });


});




 
 

