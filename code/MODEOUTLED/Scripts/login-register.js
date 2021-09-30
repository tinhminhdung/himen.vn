// JavaScript source code

// Ajax login 
    $(document).ready(function () {
        $("#btnLogin").click(function () {
            $("#err-box").attr("hidden", "hidden");
            $("#waiting-box").attr("hidden", null);
            var email = $("#email").val();
            var password = $("#password").val();
            $.post("/VCustomer/Login", {
                "email": email,
                "password": password
            },
                function (data) {
                    if (data == "success") {
                        setTimeout(function () {
                            $("#waiting-box").attr("hidden", "hidden");
                            $("#err-box").attr("hidden", null);
                        }, 1000);
                        window.location.reload();
                    } else {
                        $("#err-message").text(data);
                        setTimeout(function () {
                            $("#waiting-box").attr("hidden", "hidden");
                            $("#err-box").attr("hidden", null);
                        }, 1000);
                    }

                });
        });
    });


//Ajax register 
    $(document).ready(function () {
        $("#btnRegister").click(function () {
            $("#err-box").attr("hidden", "hidden");
            $("#waiting-box").attr("hidden", null);

            var email = $("#email").val();
            var password = $("#password1").val();
            var name = $("#name").val();
            var tel = $("#tel").val();
            var address = $("#address").val();
            $.post("/VCustomer/Register", {
                "email": email,
                "password": password,
                "name": name,
                "tel": tel,
                "address": address
            },
                function (data) {
                    if (data == "success") {      
                        setTimeout(function () {
                            $("#waiting-box").attr("hidden", "hidden");
                            $("#err-box").attr("hidden", null);
                        }, 9000);
                        window.location.reload();
                    } else {
                        $("#err-message").text(data);
                        setTimeout(function () {
                            $("#waiting-box").attr("hidden", "hidden");
                            $("#err-box").attr("hidden", null);
                        }, 1000);
                    }

                });
        });
    });
