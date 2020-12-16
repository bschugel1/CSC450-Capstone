function onPriceChange() {
    var price = document.getElementById('price-control').value;
    if (price == 0) {
        document.getElementById('alert').style.display = 'block';
    }
    else {
        document.getElementById('alert').style.display = 'none';
    }
}

function onChecked() {
    var control = document.getElementById('paymentrequired').checked;

    document.getElementById('price-control').disabled = !control;

    if (control) {
        document.getElementById('alert').style.display = 'none';
        onPriceChange();
    }
    else {
        document.getElementById('alert').style.display = 'block';
    }

 
}