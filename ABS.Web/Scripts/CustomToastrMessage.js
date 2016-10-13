
        $(document).ready(function () {
            "use strict";
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-bottom-right",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "100",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        });

        function ShowCustomToastrMessage(response) {
           
            if (response.data == 1) {
                Command: toastr["success"]("Save  Successfully!!!!");
            }
            if (response.data == -1) {
                Command: toastr["warning"]("Already Exists");
            }
            
            if (response.data == -101) {
                Command: toastr["info"]("Delete Successfully !!!! ");
            }
            if (response.data == -102) {
                Command: toastr["info"]("Update Successfully ");
            }

            if (response.data == 0) {
                Command: toastr["warning"]("Error!!!!");
            }

            if (response.data == 200) {
                Command: toastr["info"]("Approved Successfully!!!!");
            }
            if (response.data == 201) {
                Command: toastr["info"]("Declined Successfully!!!!");
            }

            if (response.data == 501) {
                Command: toastr["info"]("Please Add Details");
            }
        };


        function ShowCustomToastrMessageResult(result) {
             
            if (result == 1) {
                Command: toastr["success"]("Save  Successfully!!!!");
            }
            if (result == -1) {
                Command: toastr["warning"]("Already Exists");
            }
            if (result == -2) {
                Command: toastr["warning"]("Already Exists. Please Change Item Gorup or Count");
            }

            if (result == -101) {
                Command: toastr["info"]("Delete Successfully !!!! ");
            }
            if (result == -102) {
                Command: toastr["info"]("Update Successfully ");
            }
            
            if (result == 500) {
                Command: toastr["info"]("New Notification ");
            }
        };