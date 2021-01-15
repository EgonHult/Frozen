function CheckSelectedPayment(paymentId) {
    switch (paymentId.value) {
        case "1":
            console.log("hello");
            document.getElementById("CardOption").style.display = "block";
            document.getElementById("SwishOption").style.display = "none";
            document.getElementById("BankOption").style.display = "none";
            break;
        case "2":
            document.getElementById("CardOption").style.display = "none";
            document.getElementById("SwishOption").style.display = "block";
            document.getElementById("BankOption").style.display = "none";
            break;
        case "3":
            document.getElementById("CardOption").style.display = "none";
            document.getElementById("SwishOption").style.display = "none";
            document.getElementById("BankOption").style.display = "block";
            break;
    }
}