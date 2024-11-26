document.addEventListener("DOMContentLoaded", function () {
    const startDateInput = document.getElementById("startDate");
    const endDateInput = document.getElementById("endDate");
    const totalPriceInput = document.getElementById("totalPrice");
    const pricePerDay = parseFloat(document.getElementById("pricePerDay").value);

    function calculateTotalPrice() {
        const startDate = new Date(startDateInput.value);
        const endDate = new Date(endDateInput.value);

        if (Date(startDate.getTime()) && Date(endDate.getTime()) && startDate <= endDate) {
            const timeDifference = endDate - startDate;
            const days = Math.ceil(timeDifference / (1000 * 60 * 60 * 24));
            const totalPrice = days * pricePerDay;

            totalPriceInput.value = totalPrice.toFixed(2);
        }
        else {
            totalPriceInput.value = "Invalid dates";
        }
    }

    function setDefaultTotalPrice() {
        const defaultDays = 1;
        const totalPrice = defaultDays * pricePerDay;
        totalPriceInput.value = totalPrice.toFixed(2);
    }

    setDefaultTotalPrice();

    startDateInput.addEventListener("change", calculateTotalPrice);
    endDateInput.addEventListener("change", calculateTotalPrice);
});
