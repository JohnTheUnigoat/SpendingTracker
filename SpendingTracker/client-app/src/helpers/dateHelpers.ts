const getDatePart = (date: Date) => {
    return new Date(date.toLocaleDateString());
};

const getAmPmTime = (date: Date) => {
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
}

export {
    getDatePart,
    getAmPmTime
};