const CompareMessages = (left, right) => {
  if(left.sendTime < right.sendTime)
  {
    return -1;
  }
  else
  {
    return 1;
  }
}

export default CompareMessages;