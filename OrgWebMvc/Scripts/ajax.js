function postReq(url, param, callback, errorCallback){
	let request = new XMLHttpRequest();
	
	infoLoading();
	request.open("POST", url, true);
	request.setRequestHeader("Content-type",
			"application/x-www-form-urlencoded");
	request.onreadystatechange = function() {
		if (this.readyState == this.DONE && this.status == 200) {
			//console.log("Response",this.responseText);
			if (this.responseText != null) {
			    callback(JSON.parse(this.responseText));
				infoDone();
			}
			
		} else if (this.status == 500) {
		    errorCallback(JSON.parse(this.responseText));
		}
	}
	request.send(param);
}

function infoLoading() {
    document.getElementById("info").innerHTML = 
			 "<img width='60px' src=\"/Assets/Img/loading-disk.gif\"/><br/>Please Wait...";
}

function infoDone() {
    document.getElementById("info").innerHTML = "";
}
