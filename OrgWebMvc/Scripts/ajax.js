function postReqV2(url, param, callback, errorCallback, timeout){
	let request = new XMLHttpRequest();
	
	infoLoading();
	request.open("POST", url, true);
	if (timeout != null) {
	    request.timeout = timeout;

	}

	request.ontimeout = function (e) {
	    errorCallback(e);
	}
	request.setRequestHeader("Content-type",
			"application/x-www-form-urlencoded");
	request.onreadystatechange = function() {
		if (this.readyState == this.DONE && this.status == 200 || this.status == 201) {
			//console.log("Response",this.responseText);
			if (this.responseText != null) {
			    callback(JSON.parse(this.responseText));
				infoDone();
			}
			
		} else {
		    errorCallback((this.responseText));
		}
	}
	request.send(param);
}

function postReq(url, param, callback, errorCallback) {
    let request = new XMLHttpRequest();

    infoLoading();
    request.open("POST", url, true);
    request.setRequestHeader("Content-type",
			"application/x-www-form-urlencoded");
    request.onreadystatechange = function () {
        if (this.readyState == this.DONE && this.status == 200) {
            //console.log("Response",this.responseText);
            if (this.responseText != null) {
                callback(JSON.parse(this.responseText));
                infoDone();
            }

        } else if (this.status == 500) {
            errorCallback((this.responseText));
        }
    }
    request.send(param);
}

function infoLoading() {
    document.getElementById("loading-wrapper2").style.display = "block";
}

function infoDone() {
    document.getElementById("loading-wrapper2").style.display = "none";
}
