using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace iTVOD_WindowPhone7.TVOD.System
{
    public class SystemParameter
    {
        public static string REQUEST_SESSION_ID_FOR_LIVE_STREAMING = "http://tvod.vn/?q=external_device/get_session_id_for_live_streaming";
        public static string REQUEST_PARENT_CATEGORY = "http://tvod.vn/?q=external_device/list_parent_category_iphone";
        public static string REQUEST_CHILD_CATEGORY = "http://tvod.vn/?q=external_device/list_children_category_iphone&parent_category=";
        public static string REQUEST_VIDEO_FOLLOW_CATEGORY_PAGE = "http://tvod.vn/?q=external_device/list_video_iphone&category=%s&filter=%d&page=%p";
        public static string REQUEST_VIDEO_MOBILE_URL = "http://tvod.vn/?q=external_device/getURL_iphone&video_id=%s&video_type=3";
        public static string REQUEST_LOGIN = "http://tvod.vn/?q=external_device/login&user_name=%u&password=%p";
        public static string REQUEST_USER_PROFILE = "http://tvod.vn/?q=external_device/get_user_detail";
        public static string REQUEST_LOGOUT = "http://tvod.vn/?q=external_device/logout";
        public static string REQUEST_SEARCHING = "http://tvod.vn/?q=external_device/search&keyword=%k&page=%p";
        public static string REQUEST_LIST_DRAMA = "http://tvod.vn/?q=external_device/list_drama_iphone&filter=%f&page=%p";
        public static string REQUEST_VIDEO_FOLLOW_DRAMA = "http://tvod.vn/?q=external_device/list_video_follow_drama_iphone&drama=%d&page=%p&filter=%f";
        public static string REQUEST_PURCHASE_PACKAGE_SERVICE = "http://tvod.vn/?q=external_device/convert_balance_to_expired_date&pur_type=%t";
        public static string REQUEST_TRANSACTION = "http://tvod.vn/?q=external_device/get_list_transaction&transaction_type=%t&page=%p&time=";
        public static string REQUEST_LIST_FAVOURITE_CONTENTS = "http://tvod.vn/?q=external_device_level_2/getListContentFavorite&page=%p&time=";
        public static string REQUEST_ADD_FAVOURITE_CONTENT = "http://tvod.vn/?q=external_device_level_2/addContentFavorite&content_id=%c";
        public static string REQUEST_LIST_COMMENTS_CONTENT = "http://tvod.vn/?q=external_device_level_2/getListCommentsFollwoContent&content_id=%c&page=%p&time=";

        public static bool ENABLE_GET_DATA_FROM_ISOLATED_STORE = false;

        public static string ERROR_CODE_FROM_SERVER_NO_AUTHORIZED = "Unauthorized Access";

        public static int NUMBER_ITEMS_PER_PAGE = 25;

        public static string ID_HD_LIVE_CHILD_CATEGORY = "710";
        public static string ID_SD_LIVE_CHILD_CATEGORY = "739";
        public static string ID_MOBILE_LIVE_CHILD_CATEGORY = "740";

        public static string ID_DRAMA_CATEGORY = "4";

    }
}
