using BloodDonationPlatform.Enums;

namespace BloodDonationPlatform.Models;

public static class BloodTypeExtensions
{
    public static string ToDisplay(this BloodType type) => type switch
    {
        BloodType.APositive => "A+",
        BloodType.ANegative => "A-",
        BloodType.BPositive => "B+",
        BloodType.BNegative => "B-",
        BloodType.ABPositive => "AB+",
        BloodType.ABNegative => "AB-",
        BloodType.OPositive => "O+",
        BloodType.ONegative => "O-",
        _ => type.ToString()
    };

    public static string ToBadgeCss(this BloodType type) => type switch
    {
        BloodType.APositive => "blood-ap",
        BloodType.ANegative => "blood-an",
        BloodType.BPositive => "blood-bp",
        BloodType.BNegative => "blood-bn",
        BloodType.ABPositive => "blood-abp",
        BloodType.ABNegative => "blood-abn",
        BloodType.OPositive => "blood-op",
        BloodType.ONegative => "blood-on",
        _ => "blood-op"
    };

    public static string ToBadgeCss(this UrgencyLevel urgency) => urgency switch
    {
        UrgencyLevel.Critical => "urgency-critical",
        UrgencyLevel.Urgent => "urgency-urgent",
        _ => "urgency-routine"
    };

    public static string ToBadgeCss(this RequestStatus status) => status switch
    {
        RequestStatus.Pending => "status-pending",
        RequestStatus.Matching => "status-matching",
        RequestStatus.Fulfilled => "status-fulfilled",
        RequestStatus.Cancelled => "status-cancelled",
        _ => "status-pending"
    };

    public static bool TryParseDisplay(string? value, out BloodType bloodType)
    {
        bloodType = BloodType.OPositive;
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        switch (value.Trim().ToUpperInvariant())
        {
            case "A+":
            case "APOSITIVE":
                bloodType = BloodType.APositive;
                return true;
            case "A-":
            case "ANEGATIVE":
                bloodType = BloodType.ANegative;
                return true;
            case "B+":
            case "BPOSITIVE":
                bloodType = BloodType.BPositive;
                return true;
            case "B-":
            case "BNEGATIVE":
                bloodType = BloodType.BNegative;
                return true;
            case "AB+":
            case "ABPOSITIVE":
                bloodType = BloodType.ABPositive;
                return true;
            case "AB-":
            case "ABNEGATIVE":
                bloodType = BloodType.ABNegative;
                return true;
            case "O+":
            case "OPOSITIVE":
                bloodType = BloodType.OPositive;
                return true;
            case "O-":
            case "ONEGATIVE":
                bloodType = BloodType.ONegative;
                return true;
            default:
                return false;
        }
    }
}
