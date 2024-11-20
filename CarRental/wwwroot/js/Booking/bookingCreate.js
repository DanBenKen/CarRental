document.addEventListener("DOMContentLoaded", function () {
    const startDateInput = document.getElementById("startDate");
    const endDateInput = document.getElementById("endDate");
    const totalPriceInput = document.getElementById("totalPrice");
    const pricePerDay = parseFloat(document.getElementById("totalPrice").value);

    function calculateTotalPrice() {
        const startDate = new Date(startDateInput.value);
        const endDate = new Date(endDateInput.value);

        if (!isNaN(startDate.getTime()) && !isNaN(endDate.getTime()) && startDate <= endDate) {
            const timeDifference = endDate - startDate;
            const days = Math.ceil(timeDifference / (1000 * 60 * 60 * 24));
            const totalPrice = days * pricePerDay;

            totalPriceInput.value = totalPrice.toFixed(2);
        } else {
            totalPriceInput.value = "Invalid dates";
        }
    }

    startDateInput.addEventListener("change", calculateTotalPrice);
    endDateInput.addEventListener("change", calculateTotalPrice);
});
