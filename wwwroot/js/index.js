window.onload = (event) => {
    initMenu();
    initSvg();
    initProductPrice();
};

function initMenu() {
    const toggleClass = (el, className) => el.classList.toggle(className);
    if (document.querySelector(".menu")) {
        var hamburger = document.querySelector(".hamburger");
        var header = document.querySelector(".header");
        var mainContainerInner = document.querySelector(".main-content-inner");
        var mainOverlay = document.querySelector(".main-content-overlay");
        var headerOverlay = document.querySelector(".header-overlay");
        var menu = document.querySelector(".menu");
        var isActive = false;

        hamburger.addEventListener("click", function () {
            toggleClass(mainContainerInner, "active");
            toggleClass(menu, "active");
            toggleClass(header, "active");
            isActive = true;
        });

        mainOverlay.addEventListener("click", function () {
            if (isActive) {
                toggleClass(mainContainerInner, "active");
                toggleClass(menu, "active");
                toggleClass(header, "active");
                isActive = false;
            }
        });

        headerOverlay.addEventListener("click", function () {
            if (isActive) {
                toggleClass(mainContainerInner, "active");
                toggleClass(menu, "active");
                toggleClass(header, "active");
                isActive = false;
            }
        });
    }
}
function initSvg() {
    if (document.querySelectorAll("img.svg").length) {
        document
            .querySelectorAll("img.svg")
            .forEach(function (currentValue, index, arr) {
                var $img = currentValue;

                var imgID = $img.id;
                var imgClass = $img.className;
                var imgURL = $img.src;

                const xhttp = new XMLHttpRequest();
                xhttp.onreadystatechange = function () {
                    if (this.readyState == 4 && this.status == 200) {
                        var $svg = this.responseXML.querySelectorAll("svg");

                        if (typeof imgID !== "undefined") {
                            $svg[0].id = $svg[0].id;
                        }
                        if (typeof imgClass !== "undefined") {
                            $svg[0].setAttribute("class", imgClass + " replaced-svg");
                        }
                        $svg[0].removeAttribute("xmlns:a");

                        $img.replaceWith($svg[0]);
                    }
                };
                xhttp.open("GET", imgURL);
                xhttp.send();
            });
    }
}

function initProductPrice() {
    if (document.querySelector(".product-detail-price")) {
        var product_detail_price = document.querySelector(".product-detail-price");
        var product_price = parseInt(product_detail_price.innerHTML);
        product_detail_price.innerHTML =
            formatter.format(product_price) + "<span>.00</span>";
    }

    if (document.querySelectorAll(".product-price").length) {
        document
            .querySelectorAll(".product-price")
            .forEach(function (currentValue, index, arr) {
                var value = parseInt(currentValue.innerHTML);
                currentValue.innerHTML = formatter.format(value) + "<span>.00</span>";
            });
    }

    if (document.querySelectorAll(".cart-product-price").length) {
        document
            .querySelectorAll(".cart-product-price")
            .forEach(function (currentValue, index, arr) {
                var value = parseInt(currentValue.innerHTML);
                currentValue.innerHTML = formatter.format(value) + ".00";
            });
    }

    if (document.querySelectorAll(".cart-product-total").length) {
        document
            .querySelectorAll(".cart-product-total")
            .forEach(function (currentValue, index, arr) {
                var value = parseInt(currentValue.innerHTML);
                currentValue.innerHTML = formatter.format(value) + ".00";
            });
    }
}

var formatter = new Intl.NumberFormat("th-TH", {
    style: "currency",
    currency: "THB",
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
});
