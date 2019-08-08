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
let begin = { week: 1, day: 3, dayCount: 31 };
let begin_old = { week: 0, day: 0, dayCount: 0 };
let year_now = 1945;
//let month_label = document.getElementById("month");
//let year_label = document.getElementById("year");
let tabel = document.getElementById("date");
let input_month = document.getElementById("input_month");
let input_year = document.getElementById("input_year");
let date_info = document.getElementById("date-info");
let running_month = 7;
let running_year = 1945;

function load() {
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
        console.log("option ", i, input_month);
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
            tr.appendChild(col);
        }
        tBody.appendChild(tr);
    }
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
            nextMonth();
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
            prevMonth();
            ////console.log("b",b,"month",current_month);
            current_month--;
        }

    }

    fillInfo();
    console.log("==end==")
}

function fillInfo() {
    date_info.innerHTML = month[month_now].name + " " + year_now;
}

function detail(day, month, year) {
    console.log("DETAIL", day, month, year);
}

function prevMonth() {
    doPrevMonth(false);
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

}

function nextMonth() {
    doNextMonth(false);
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
            dates[i].innerHTML = day;
            dates[i].id = "date-"+day;
            dates[i].setAttribute("onclick", "detail(" + day + "," + month_now+1 + "," + year_now + ")");
        } 

    }
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
    console.log("isNow", isNow,running_month,'=',current_month, running_month == current_month,running_year,'=',year_now, running_year == year_now)
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
    return begin_new;
}