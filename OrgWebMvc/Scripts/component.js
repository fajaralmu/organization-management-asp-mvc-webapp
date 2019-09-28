function isElementVisible(el) {
    let top = el.offsetTop;
    let left = el.offsetLeft;
    let width = el.offsetWidth;
    let height = el.offsetHeight;

  //  console.log(top, left, width, height);
    while (el.offsetParent) {
        el = el.offsetParent;
         top += el.offsetTop;
         left += el.offsetLeft;
    }
    return (
    top >= window.pageYOffset &&
    left >= window.pageXOffset &&
    (top + height) <= (window.pageYOffset + window.innerHeight) &&
    (left + width) <= (window.pageXOffset + window.innerWidth)
  );
}