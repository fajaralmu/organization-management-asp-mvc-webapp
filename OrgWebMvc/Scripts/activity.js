let bulan = [
        { nama: "Januari", hari: 31 },
        { nama: "Februari", hari: 28 },
        { nama: "Maret", hari: 31 },
        { nama: "April", hari: 30 },
        { nama: "Mei", hari: 31 },
        { nama: "Juni", hari: 30 },
        { nama: "Juli", hari: 31 },
        { nama: "Agustus", hari: 31 },
        { nama: "September", hari: 30 },
        { nama: "Oktober", hari: 31 },
        { nama: "November", hari: 30 },
        { nama: "Desember", hari: 31 },
];
let bulan_skrg = 7;// 0;
//let mulai = { pekan: 2, hari: 1, jmlhari: 31 };
let mulai = { pekan: 1, hari: 3, jmlhari: 31 };
let mulai_lama = { pekan: 0, hari: 0, jmlhari: 0 };
let tahun_skrg = 1945;
let bulan_label = document.getElementById("bulan");
let tahun_label = document.getElementById("tahun");
let tabel = document.getElementById("tgl");
let input_bulan = document.getElementById("bulan");
let input_tahun = document.getElementById("tahun");
let tgl_info = document.getElementById("tgl-info");

function load() {
    alert("oke");
    ////console.log("Now Month ", bulan_skrg);
    // //console.log("Month  ", bulan[bulan_skrg])
    bulan_label.innerHTML = bulan[bulan_skrg].nama;
    buatTabel();
    mulai_lama = mulai;
    mulai = isiHari(bulan_skrg, true, mulai);

    input_bulan.value = new Date().getMonth() + 1;
    input_tahun.value = new Date().getFullYear();
    cari();
    
}

function buatTabel() {
    //console.log("BUAT TABEL");
    let tBody = document.createElement("tbody");
    for (let r = 1; r <= 6; r++) {
        let tr = document.createElement("tr");
        // tr.setAttribute("pekan",r);
        for (let i = 1; i <= 7; i++) {
            let col = document.createElement("td");
            col.setAttribute("class", "_tanggal_");
            col.setAttribute("hari", +i);
            col.setAttribute("pekan", +r);
            tr.appendChild(col);
        }
        tBody.appendChild(tr);
    }
    tabel.appendChild(tBody);
}



function cari() {
    doCari();
}



function doCari() {
    let bln = input_bulan.value - 1;
    let thn = input_tahun.value;
    let selisih = +Math.abs(thn - tahun_skrg);
    alert("Selisih tahun:" + selisih);
    let jmlbulan = 0;
    if (selisih > 0)
        jmlbulan = (11 - bulan_skrg) + (selisih > 1 ? ((selisih - 1) * 12) : 0) + (+bln);
    else
        jmlbulan = bln - bulan_skrg;
    let kurangdari = false;
    if (thn - tahun_skrg > 0) {
        kurangdari = false;
    } else if (thn - tahun_skrg < 0) {
        kurangdari = true;
    } else {
        if (bln - bulan_skrg > 0) {
            kurangdari = false;
        } else {
            kurangdari = true;
        }
    }
    jmlbulan = Math.abs(jmlbulan);
    //console.log("kurang dari: ", kurangdari);
    let b_calc = bulan_skrg;
    let to = (jmlbulan + bulan_skrg);
    if (jmlbulan <= 0)
        return;
    if (!kurangdari)
        ////console.log("bulan skrg",bulan_skrg,"selisih",jmlbulan,"to",to);
    {
        for (let b = bulan_skrg + 1; b <= to + 1; b++) {
            if (b_calc > 11) {
                b_calc = 0;

            }
            bulan_skrg = b_calc;
            nextmonth();
            ////console.log("bulan",b_calc,"thn",tahun_skrg);
            b_calc++;
        }
    } else if (kurangdari) {
        let jmlbulankeblkg = (bulan_skrg) + (selisih > 1 ? ((selisih - 1) * 12) : 0) + (11 - bln);
        to = (jmlbulankeblkg + bulan_skrg);
        //console.log("bulan skrg", bulan_skrg, "selisih", jmlbulan, "from", to);
        let mulai_bulan = bulan_skrg;
        for (let b = to + 1; b >= mulai_bulan + 1; b--) {
            if (b_calc < 0) {
                b_calc = 11;
            }
            bulan_skrg = b_calc;
            prefmonth();
            ////console.log("b",b,"bulan",b_calc);
            b_calc--;
        }

    }

    fillInfo();

}

function fillInfo() {
    tgl_info.innerHTML = bulan[bulan_skrg].nama + " " + tahun_skrg;
}

function detail(hari, bulan, thn) {
    //console.log(hari, bulan, thn);
}

function prefmonth() {
    bulan_skrg--;
    if (bulan_skrg < 0) {
        bulan_skrg = 11;
        tahun_skrg--;
    }
    let mulai_prev = cariMulai(mulai_lama, mulai_lama.jmlhari);
    //console.log("LAMA", mulai_lama.hari, mulai_lama.pekan, "prev");
    //console.log("MULAI_PREV", mulai_prev.hari, mulai_prev.pekan, "prev");
    mulai_lama = {
        pekan: mulai_prev.pekan,
        hari: mulai_prev.hari,
        jmlhari: mulai_prev.jmlhari,
    }
    let switch_ = isiHari(bulan_skrg, false, mulai_prev);
    mulai = {
        pekan: switch_.pekan,
        hari: switch_.hari,
        jmlhari: switch_.jmlhari
    }

}

function cariMulai(mulai_lama_, totalhari) {
    let b = bulan_skrg - 1;
    if (b < 0) {
        b = 11;
    }
    let hari = mulai_lama_.hari;
    let pekan = 6;
    let mulai_prev_ = {
        pekan: 0,
        hari: 0,
        jmlhari: bulan[b].hari
    }

    for (let h = totalhari; h >= 0; h--) {
        if (hari <= 0) {
            hari = 7;
            pekan--;
        }
        hari--;
    }
    mulai_prev_.pekan = pekan;
    mulai_prev_.hari = hari + 1;
    return mulai_prev_;
}


function nextmonth() {
    bulan_skrg++;
    if (bulan_skrg > 11) {
        bulan_skrg = 0;
        tahun_skrg++;
    }

    let switch_ = isiHari(bulan_skrg, true, mulai);
    mulai_lama = {
        pekan: mulai.pekan,
        hari: mulai.hari,
        jmlhari: mulai.jmlhari,
    }
    mulai = {
        pekan: switch_.pekan,
        hari: switch_.hari,
        jmlhari: switch_.jmlhari,
    }

}

function setElementByAttr(attr, val, attr2, val2, h) {
    let dates = document.getElementsByClassName("_tanggal_");
    let a = 0;
    for (let i = 0; i < dates.length; i++) {
        let cek = dates[i].getAttribute(attr) == val;
        if (cek) {
            let cek2 = dates[i].getAttribute(attr2) == val2;
            if (cek2) {
                dates[i].innerHTML = h;
                dates[i].setAttribute("onclick", "detail(" + h + "," + bulan_skrg + "," + tahun_skrg + ")");
            }
        }
    }
}

function clear() {
    let dates = document.getElementsByClassName("_tanggal_");
    let a = 0;
    for (let i = 0; i < dates.length; i++) {
        dates[i].innerHTML = "";

    }
    if (+tahun_skrg % 4 == 0) {
        bulan[1].hari = 29
    } else {
        bulan[1].hari = 28
    }
    tahun_label.innerHTML = tahun_skrg;
    bulan_label.innerHTML = bulan[bulan_skrg].nama;
}

function isiHari(bulan_skrg, next, mulai) {
    clear();
    let mulai_baru = {
        pekan: mulai.pekan,
        hari: mulai.hari,
        jmlhari: mulai.jmlhari
    };
    let mulai_lama_ = {
        pekan: mulai.pekan,
        hari: mulai.hari,
        jmlhari: mulai.jmlhari
    };
    let pekan_ = mulai_baru.pekan;
    let mulai_pekan = pekan_;
    if (mulai_baru.pekan > 1 && mulai_baru.hari > 1) {
        pekan_ = 1;
        mulai_pekan = 1;
    }
    let hari_ = mulai_baru.hari;
    let mulai_hari = hari_;
    for (let h = 1; h <= bulan[bulan_skrg].hari; h++) {
        if (hari_ > 7) {
            hari_ = 1;
            pekan_++;
        }
        setElementByAttr("pekan", pekan_, "hari", hari_, h);
        hari_++;
    }
    mulai_baru.pekan = pekan_ >= 5 ? 2 : 1;
    mulai_baru.hari = hari_;
    mulai_baru.jmlhari = bulan[bulan_skrg].hari;
    //console.log("LAMA", mulai_lama_.hari, mulai_lama_.pekan);
    //console.log("   ");
    //console.log("BARU", mulai_baru.hari, mulai_baru.pekan);
    fillInfo();
    return mulai_baru;
}