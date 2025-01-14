const FormatDate = (date) => {
  console.log(date);
  const dateObj = new Date(date);

  if(isNaN(dateObj.getTime()))
  {
    return "Invalid Date";
  }

  const day = dateObj.getDate();
  const month = dateObj.toLocaleString('default', { month: 'short' });
  // const year = dateObj.getUTCFullYear();
  const hours = dateObj.getHours();
  const minues = dateObj.getMinutes();

  return `${minues}:${hours}:${day}:${month}`;
}

export default FormatDate;