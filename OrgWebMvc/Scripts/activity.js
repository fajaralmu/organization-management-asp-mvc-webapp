let month = [
        { name: "Januari", day: 31 },
        { name: "Februari", day: 28 },
        { name: "Maret", day: 31 },
        { name: "April", day: 30 },
        { name: "Mei", day: 31 },
        { name: "Juni", day: 30 },
        { name: "Juli", day: 31 },
        { name: "Agustus", day: 31 },
        { name: "September", day: 30 },
        { name: "Oktober", day: 31 },
        { name: "November", day: 30 },
        { name: "Desember", day: 31 },
];
let month_now = 7;// 0;
//let begin = { week: 2, day: 1, dayCount: 31 };
let begin = { week: 1, day: 3, dayCount: 31, info: "" };
let begin_old = { week: 0, day: 0, dayCount: 0, info: "" };
let year_now = 1945;
//let month_label = document.getElementById("month");
//let year_label = document.getElementById("year");
let tabel = document.getElementById("calendarTable");
let input_month = document.getElementById("input_month");
let input_year = document.getElementById("input_year");
let date_info = document.getElementById("date-info");
let running_month = 7;
let running_year = 1945;
var filterDayId, filterMonthId, filterYearId;
var entity_Name, entity_Prop, dateFormId;


function initEntity(entityName, propName, dateID) {
    this.entity_Name = entityName;
    this.entity_Prop = propName;
    console.log("entity Name and prop ", entity_Name, entity_Prop);
    setDateElementId(dateID);

}

function setDateElementId(dateID) {
    console.log("Set date id field ",dateID);
    dateFormId = dateID;
    let filterStr = "filter-" + dateID;
    filterDayId = (filterStr+".day");
    filterMonthId = (filterStr + ".month");
    filterYearId = (filterStr + ".year");
}

function loadCalendar() {
    createTable();
    begin_old = begin;
    begin = fillDay(month_now, true, begin);
    fillInputMonth();
    input_month.value = new Date().getMonth() + 1;
    input_year.value = new Date().getFullYear();
    setCalendar();

}

function fillInputMonth() {
    input_month.innerHTML = "";
    for (let i = 0; i < month.length; i++) {
        //  console.log("option ", i, input_month);
        let opt = document.createElement("option");
        opt.value = i + 1;
        opt.innerHTML = month[i].name;
        input_month.appendChild(opt);
    }
}

function createTable() {
    //console.log("BUAT TABEL");
    let tBody = document.createElement("tbody");
    for (let r = 1; r <= 6; r++) {
        let tr = document.createElement("tr");
        // tr.setAttribute("week",r);
        for (let i = 1; i <= 7; i++) {
            let col = document.createElement("td");
            col.setAttribute("class", "date_element");
            col.setAttribute("day", +i);
            col.setAttribute("week", +r);
            col.style.wordWrap = "normal";

            tr.appendChild(col);
        }
        tBody.appendChild(tr);
    }
    tabel.className = "table table-bordered";
    tabel.style.tableLayout = "fixed";
    tabel.appendChild(tBody);
}

function setCalendar() {
    //  loading();
    //  cariAsync();
    doSetCalendar();
}

function doSetCalendar() {
    console.log("==start==");

    running_month = input_month.value - 1;
    running_year = input_year.value;
    let diff_year = +Math.abs(running_year - year_now);
    // alert("diff_year year:" + diff_year);
    let monthCount = 0;
    if (diff_year > 0)
        monthCount = (11 - month_now) + (diff_year > 1 ? ((diff_year - 1) * 12) : 0) + (+running_month);
    else
        monthCount = running_month - month_now;
    let less = false;
    if (running_year - year_now > 0) {
        less = false;
    } else if (running_year - year_now < 0) {
        less = true;
    } else {
        if (running_month - month_now > 0) {
            less = false;
        } else {
            less = true;
        }
    }
    monthCount = Math.abs(monthCount);
    //console.log("kurang dari: ", less);
    let current_month = month_now;
    let endMonth = (monthCount + month_now);
    if (monthCount <= 0)
        return;
    if (!less)
        ////console.log("month now",month_now,"diff_year",monthCount,"to",to);
    {
        for (let m = month_now + 1; m <= endMonth + 1; m++) {
            if (current_month > 11) {
                current_month = 0;

            }
            month_now = current_month;
            let end = nextMonth();
            if (end) {
                break;
            }
            ////console.log("month",current_month,"running_year",year_now);
            current_month++;
        }
    } else if (less) {
        let pastMonthCount = (month_now) + (diff_year > 1 ? ((diff_year - 1) * 12) : 0) + (11 - running_month);
        endMonth = (pastMonthCount + month_now);
        //console.log("month now", month_now, "diff_year", monthCount, "from", to);
        let begin_month = month_now;
        for (let b = endMonth + 1; b >= begin_month + 1; b--) {
            if (current_month < 0) {
                current_month = 11;
            }
            month_now = current_month;
            let end = prevMonth();
            if (end) {
                break;
            }
            ////console.log("b",b,"month",current_month);
            current_month--;
        }

    }

    fillInfo();
    console.log("==end==")
}

function fillInfo() {
    date_info.value = month[month_now].name + " " + year_now;
}

function clearDateFilter() {
    detail("", "", "");
}

function detail(day, month, year) {
    let filterDay = document.getElementById(filterDayId);
    let filterMonth = document.getElementById(filterMonthId);
    let filterYear = document.getElementById(filterYearId);

    console.log("DETAIL", day, month, year);
    if (filterDay == null || filterMonth == null || filterYear == null) {
        console.log("NULL", filterDayId, filterMonthId, filterYearId)
        return;
    }
    filterDay.value = day;
    filterMonth.value = month;
    filterYear.value = year;

    filterEntity(null, null, null);

    loadList("externalRequest");
    loadJSON();
}

function loadJSON() {
    let params = "Type=JSONList&Action=List&limit=" + limit + "&offset=" + offset;
    if (search_params != null) {
        params += "&search_param=${" + search_params + "}$";
    }
    let svcURL = "/Entity/" + entity_Name + "Svc";
    console.log("Svc URL", svcURL);
    postReq(svcURL,
         params,
         function (data) {
             //    console.log("JSON response", data);
             if (data.code == 0) {
                 fillEventData(data.data);
             }
         }, function (data) {
             console.log("API error", data);
         });
}

function fillEventData(eventList) {
    let dateCells = document.getElementsByClassName("date_element");
    for (let i = 0; i <= 31; i++) {
        if (document.getElementById("date-list-" + i) != null)
            document.getElementById("date-list-" + i).innerHTML = "";
    }

    for (let i = 0; i < eventList.length; i++) {

        let event = eventList[i];
        var evDate = event.date.replace('/', '').replace("Date", "").replace('(', '').replace(')', '').replace('/', '');
        let day = +(evDate) / (24 * 60 * 60 * 1000);
        let date = new Date(+evDate);
        let dateCell = document.getElementById("date-list-" + date.getDate());

        if (dateCell.getElementsByTagName("li") != null && dateCell.getElementsByTagName("li").length >= 3) {
            console.log("more than 3");
            console.log("-");
            if (dateCell.getElementsByTagName("code").length == 0) {
                let info = document.createElement("code");
                info.innerHTML = "click details <br/>to see more";
                dateCell.appendChild(info);
            }
            continue;
        }
        //  console.log("ID: ", "date-list-" + date.getDate(), date)
        //console.log("-");
        let li = document.createElement("li");
        li.className = "li-custom li-uncheck";
        let evtName = document.createElement("span");
        evtName.innerHTML = event[entity_Prop];
        evtName.className = "text-wrap";
        evtName.style.fontFamily = "Calibri";
        //  evtName.style.color = "black";

        if (event.done == 1 || event.done == "1") {
            li.className = "li-custom li-checked";
        }
        li.appendChild(evtName);

        dateCell.appendChild(li);
    }
}

function prevMonth() {
    return doPrevMonth(false);
}

function doPrevMonth(prev) {
    month_now--;
    if (prev) {
        running_month--;
    }
    if (month_now < 0) {
        month_now = 11;
        year_now--;
        if (prev) {
            running_month = 11;
            running_year--;
        }
    }
    let begin_prev = caribegin(begin_old, begin_old.dayCount);
    //console.log("old", begin_old.day, begin_old.week, "prev");
    //console.log("begin_PREV", begin_prev.day, begin_prev.week, "prev");
    begin_old = {
        week: begin_prev.week,
        day: begin_prev.day,
        dayCount: begin_prev.dayCount,
    }
    let switch_ = fillDay(month_now, false, begin_prev);
    begin = {
        week: switch_.week,
        day: switch_.day,
        dayCount: switch_.dayCount
    }
    return switch_.info == "NOW";
}

function nextMonth() {
    return doNextMonth(false);
}

function doNextMonth(next) {
    month_now++;
    if (next) {
        running_month++;
    }
    if (month_now > 11) {
        month_now = 0;
        year_now++;
        if (next) {
            running_month = 0;
            running_year++;
        }
    }

    let switch_ = fillDay(month_now, true, begin);
    begin_old = {
        week: begin.week,
        day: begin.day,
        dayCount: begin.dayCount,
    }
    begin = {
        week: switch_.week,
        day: switch_.day,
        dayCount: switch_.dayCount,
    }
    return switch_.info == "NOW";

}

function caribegin(begin_old_, totalday) {
    let M = month_now - 1;
    if (M < 0) {
        M = 11;
    }
    let day = begin_old_.day;
    let week = 6;
    let begin_prev_ = {
        week: 0,
        day: 0,
        dayCount: month[M].day
    }

    for (let D = totalday; D >= 0; D--) {
        if (day <= 0) {
            day = 7;
            week--;
        }
        day--;
    }
    begin_prev_.week = week;
    begin_prev_.day = day + 1;
    return begin_prev_;
}

function setElementByAttr(attr, val, attr2, val2, day) {
    let dates = document.getElementsByClassName("date_element");
    let a = 0;
    for (let i = 0; i < dates.length; i++) {
        let cek = dates[i].getAttribute(attr) == val;
        let cek2 = dates[i].getAttribute(attr2) == val2;
        if (cek && cek2) {
            dates[i].innerHTML = "";
            dates[i].id = "date-" + day;
            if (new Date().getDate() == day && new Date().getMonth() == month_now && new Date().getYear() + 1900 == year_now) {
                console.log("NOW", i);
                dates[i].setAttribute("style", "background-color:yellow");
            } else {
                dates[i].setAttribute("style", "background-color:white");
                console.log("NOT NOW", i);
            }
            let dateStr = addZero(day, 10).concat("-").concat(addZero((+month_now + 1), 10)).concat("-").concat(year_now);

            let addBtn = document.createElement("code");
            addBtn.innerHTML = "+";
            addBtn.className = "btn btn-default btn-xs";
            addBtn.style.cssFloat = "right";
            addBtn.setAttribute("data-toggle", "tooltip");
            addBtn.setAttribute("title", "Add an event at " + dateStr + "!");
            addBtn.setAttribute("onclick", "addNewEvent(" + day + "," + (+month_now + 1) + "," + year_now + ")");
            dates[i].appendChild(addBtn);

            let detailBtn = document.createElement("code");
            detailBtn.innerHTML = "&#10296;";
            detailBtn.className = "btn btn-default btn-xs";
            detailBtn.style.cssFloat = "right";
            detailBtn.setAttribute("data-toggle", "tooltip");
            detailBtn.setAttribute("title", "See events at " + dateStr + "!");
            detailBtn.setAttribute("onclick", "detail(" + day + "," + (+month_now + 1) + "," + year_now + ")");
            dates[i].appendChild(detailBtn);

            let span = document.createElement("span");
            span.innerHTML = day;
            dates[i].appendChild(span);

            let ul = document.createElement("ul");
            ul.style.listStyle = "none";
            ul.id = "date-list-" + day;
            dates[i].appendChild(ul);



        }

    }

}

function addNewEvent(day, month, year) {
    showMenu('entity-form', document.getElementById('btn-show-form'));
    let strDate = dateAcceptableForHtmlInput(day, month, year);
    document.getElementById(dateFormId).value = strDate;
}

function dateAcceptableForHtmlInput(day, month, year) {
    return year + "-" + addZero(month, 10) + "-" + addZero(day, 10);
}

function addZero(Val, Min) {
    let N = new String(Val);
    let MinStr = new String(Min);

    let ValLength = N.length;
    let MinLength = MinStr.length;
    let Diff = MinLength - ValLength;
    for (let i = 1; i <= Diff; i++) {
        N = new String("0").concat(N);
    }

    return N;
}

function clear() {
    let dates = document.getElementsByClassName("date_element");
    let a = 0;
    for (let i = 0; i < dates.length; i++) {
        dates[i].innerHTML = "";
    }
    month[1].day = 28 + (+year_now % 4 == 0 ? 1 : 0);
}

function fillDay(current_month, next, begin) {
    clear();
    let begin_new = {
        week: begin.week,
        day: begin.day,
        dayCount: begin.dayCount
    };
    let begin_old_ = {
        week: begin.week,
        day: begin.day,
        dayCount: begin.dayCount
    };
    let week_ = begin_new.week;
    let begin_week = week_;
    if (begin_new.week > 1 && begin_new.day > 1) {
        week_ = 1;
        begin_week = 1;
    }
    let day_ = begin_new.day;
    let begin_day = day_;
    let isNow = running_month == current_month && running_year == year_now;
    //  console.log("isNow", isNow,running_month,'=',current_month, running_month == current_month,running_year,'=',year_now, running_year == year_now)
    for (let d = 1; d <= month[current_month].day; d++) {
        if (day_ > 7) {
            day_ = 1;
            week_++;
        }
        if (isNow) {
            setElementByAttr("week", week_, "day", day_, d);
        }
        day_++;
    }
    begin_new.week = week_ >= 5 ? 2 : 1;
    begin_new.day = day_;
    begin_new.dayCount = month[current_month].day;
    //console.log("old", begin_old_.day, begin_old_.week);
    //console.log("   ");
    //console.log("new", begin_new.day, begin_new.week);
    fillInfo();
    if (isNow) {
        detail(null, (+running_month + 1), running_year);
        begin_new.info = "NOW";
    } else {
        begin_new.info = "SOME-DAY";
    }
    return begin_new;
}