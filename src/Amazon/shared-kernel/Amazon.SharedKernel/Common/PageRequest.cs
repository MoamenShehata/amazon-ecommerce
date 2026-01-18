namespace Amazon.SharedKernel.Common;

public record PageRequest(int PageNumber = 1, int PageSize = 30, string LastSeenValue = null);
