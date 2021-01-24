using MeowIOTBot.NetworkHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MeowIOTBot.NetworkHelper.PostHelper;
using static MeowIOTBot.QQ.QQFunc.QQGroupList;
using static MeowIOTBot.QQ.QQFunc.QQGroupUserList;

/// <summary>
/// 总操作函数库
/// <para>Controller of QQ Action</para>
/// </summary>
namespace MeowIOTBot.QQ.QQFunc
{
    /// <summary>
    /// 用户的Cookie[字段未识别用途,保留]
    /// <para>User's Cookies the Parameter is not in instance for usage</para>
    /// </summary>
    public class QQCookie
    {
        /// <summary>
        /// 
        /// </summary>
        public string ClientKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Cookies { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Gtk { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Gtk32 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Pskey PSkey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Skey { get; set; }
        /// <summary>
        /// Inner Class for cookie.//
        /// </summary>
        public class Pskey
        {
            /// <summary>
            /// 
            /// </summary>
            public string connect { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string docs { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string docx { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string game { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string gamecenter { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string imgcache { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string mtencentcom { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string mail { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string mma { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string now { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string office { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string openmobile { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string qqweb { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string qun { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string qzone { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string qzonecom { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string tenpaycom { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ti { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string vip { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string weishi { get; set; }
        }
    }

    /// <summary>
    /// QQ用户
    /// </summary>
    public class QQPerson
    {
        /// <summary>
        /// 代码
        /// <para>Code parameter</para>
        /// </summary>
        public int code;
        /// <summary>
        /// 存储区
        /// <para>it's which the DataStores</para>
        /// </summary>
        public datax data;
        /// <summary>
        /// 储存用的类
        /// <para>the Class that holds the Data</para>
        /// </summary>
        public class datax
        {
            /// <summary>
            /// 头像地址
            /// <para>the avator url</para>
            /// </summary>
            public string avatarUrl;
            /// <summary>
            /// 头像bitmapHash
            /// <para>avator Bitmap Hash</para>
            /// </summary>
            public string bitmap;
            /// <summary>
            /// 保留字段
            /// <para>some unidentify parameter</para>
            /// </summary>
            public int commfrd;
            /// <summary>
            /// 朋友状态
            /// <para>a statues that indecate friendship-status</para>
            /// </summary>
            public int friendship;
            /// <summary>
            /// 所在地
            /// <para>a parameter that indecate where its from(s)</para>
            /// </summary>
            public string from;
            /// <summary>
            /// 性别
            /// <para>Friend Gender</para>
            /// </summary>
            public int gender;
            /// <summary>
            /// 活跃值
            /// <para>it's a value that indecate your friends QQ using frequency</para>
            /// </summary>
            public long intimacyScore;
            /// <summary>
            /// 是否为朋友
            /// <para>it's a parameter that if you two is friend</para>
            /// </summary>
            public int isFriend;
            /// <summary>
            /// --
            /// </summary>
            public string logolabel;
            /// <summary>
            /// 昵称
            /// <para>a parameter that shows your friend nickname</para>
            /// </summary>
            public string nickname;
            /// <summary>
            /// 腾讯会员等级状态
            /// <para>is a parameter shows that your friend is a VIP</para>
            /// </summary>
            public int qqvip;
            /// <summary>
            /// 是否开通QQ空间
            /// <para>is a parameter that indecate is your friend have QZone Opened</para>
            /// </summary>
            public int qzone;
            /// <summary>
            /// 真名
            /// <para>it's a parameter that indecate your friend's Realname</para>
            /// </summary>
            public string realname;
            /// <summary>
            /// 其他名
            /// <para>it's a parameter that indecate is you friend is having some other nick name</para>
            /// </summary>
            public string smartname;
            /// <summary>
            /// QQ号
            /// <para>user QQ number</para>
            /// </summary>
            public long uin;
        }
        /// <summary>
        /// 默认行为
        /// <para>in progress default function place</para>
        /// </summary>
        public int @default;
        /// <summary>
        /// 返回的查询信息
        /// <para>the result of query</para>
        /// </summary>
        public string message;
        /// <summary>
        /// 子串码
        /// <para>the next gen of query code</para>
        /// </summary>
        public int subcode;
    }
    /// <summary>
    /// 好友列表
    /// <para>get QQFriendList</para>
    /// </summary>
    public class QQFriendList
    {
        /// <summary>
        /// QQ好友类
        /// <para>this is a class that indecate your QQ friend</para>
        /// </summary>
        public class QQFriend
        {
            /// <summary>
            /// 好友QQ号
            /// <para>QQ number</para>
            /// </summary>
            public long FriendUin;
            /// <summary>
            /// 是否含有备注
            /// <para>if you get this Friend Remarked</para>
            /// </summary>
            public bool IsRemark;
            /// <summary>
            /// 好友昵称
            /// <para>your friends nickname</para>
            /// </summary>
            public string NickName;
            /// <summary>
            /// 在线字符串
            /// <para>this is a string that your friend that Online choose</para>
            /// </summary>
            public string OnlineStr;
            /// <summary>
            /// 昵称
            /// <para>this is the remark that you may give to your </para>
            /// </summary>
            public string Remark;
            /// <summary>
            /// 状态
            /// <para>status</para>
            /// </summary>
            public int status;
        }
        /// <summary>
        /// 总好友数
        /// <para>the full count of your friend</para>
        /// </summary>
        public int Friend_count;
        /// <summary>
        /// 被调用好友数
        /// <para>the sequence that your Indecator have</para>
        /// </summary>
        public List<QQFriend> FriendList;
        /// <summary>
        /// 获取好友数
        /// <para>the friend that is using to get</para>
        /// </summary>
        public int GetfriendCount;
        /// <summary>
        /// 列表读取
        /// <para>List Start Index</para>
        /// </summary>
        public int StartIndex;
        /// <summary>
        /// 好友总数
        /// <para>Total Friend Count</para>
        /// </summary>
        public int Totoal_friend_count;
        /// <summary>
        /// 内部保存的SET集
        /// <para>the Set of all indecator</para>
        /// </summary>
        public class _FriendList
        {
            private List<QQFriend> q = new List<QQFriend>();
            /// <summary>
            /// 添加集方法
            /// <para>Set Insert Function</para>
            /// </summary>
            /// <param name="n">项目</param>
            /// <returns></returns>
            public bool Add(QQFriend n)
            {
                if (q.Exists((QQFriend k) => k.FriendUin == n.FriendUin))
                {
                    return false;
                }
                else
                {
                    q.Add(n);
                    return true;
                }
            }
            /// <summary>
            /// 获取集
            /// <para>get Set</para>
            /// </summary>
            /// <returns></returns>
            public List<QQFriend> getInstance() => this.q;
        }

        /// <summary>
        /// 获取列表的部分集合
        /// <para>get partial list of indecator SETs</para>
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static async Task<QQFriendList> GetListOnIndex(int Index) => JsonConvert.DeserializeObject<QQFriendList>(await PASA(UrlType.GetQQUserList, $"{{\"StartIndex\":{Index}}}"));
    }
    /// <summary>
    /// QQ好友请求(管理类)方法
    /// <para>QQ Friend Request Response Method</para>
    /// </summary>
    public class QQFriendRequest
    {
        /// <summary>
        /// 加好友总方法
        /// <para>Add Friend Method Set</para>
        /// </summary>
        /// <param name="AddFromSource">
        /// 2011 空间 2020 QQ搜索 2004 群组 2005 讨论组
        /// <para>2011:Qzone 2020:QQSearch 2004:FromSpecificGroup 2005:FromDiscussGroup</para>
        /// </param>
        /// <param name="FromGroupID">
        /// 来源为2004时需要添加
        /// <para>the Group you both in, which is needed for Type 2004</para>
        /// </param>
        /// <param name="Content">
        /// 内容
        /// <para>the content that you want to present</para>
        /// </param>
        /// <param name="AddUserUid">
        /// 要添加的用户
        /// <para>the User that you want to install with</para>
        /// </param>
        /// <returns></returns>
        public static async Task<string> __FriendManage(int AddFromSource, long FromGroupID, long AddUserUid, string Content) =>
                await PASA(
                    UrlType.AddQQUser,
                    $"{{\"AddUserUid\":{AddUserUid}," +
                    $"\"Content\":\"{Content}\"," +
                    $"\"AddFromSource\":{AddFromSource}," +
                    $"\"FromGroupID\":{FromGroupID}}}"
                );
    }

    /// <summary>
    /// 群成员列表
    /// <para>QQ GroupMember List</para>
    /// </summary>
    public class QQGroupUserList
    {
        /// <summary>
        /// 列对象
        /// <para>the counter object int</para>
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 群号
        /// <para>the GroupNumber</para>
        /// </summary>
        public long GroupUin { get; set; }
        /// <summary>
        /// 保留
        /// <para>the Programme running need this parameter to check</para>
        /// </summary>
        public int LastUin { get; set; }
        /// <summary>
        /// 群成员列表
        /// <para>GroupMember List</para>
        /// </summary>
        public List<QQGroupUser> MemberList { get; set; }
        /// <summary>
        /// QQ群成员
        /// <para>the Group member Class</para>
        /// </summary>
        public class QQGroupUser
        {
            /// <summary>
            /// 年龄
            /// <para>Age</para>
            /// </summary>
            public int Age;
            /// <summary>
            /// 未知属性
            /// <para>some undefined code</para>
            /// </summary>
            public int FaceId;
            /// <summary>
            /// 性别
            /// <para>gender</para>
            /// </summary>
            public int Gender;
            /// <summary>
            /// 群昵称
            /// <para>his group nickname</para>
            /// </summary>
            public string GroupCard;
            /// <summary>
            /// 加入群时间
            /// <para>the Time that join in Group</para>
            /// </summary>
            public long JoinGroupTime;
            /// <summary>
            /// 展示的群成员邮箱
            /// <para>The Email it choose</para>
            /// </summary>
            public string MemberEmail;
            /// <summary>
            /// 群成员QQ号
            /// <para>the QQ number that Member have</para>
            /// </summary>
            public long MemberUin;
            /// <summary>
            /// 群成员昵称
            /// <para>the nickname of the specific (QQNickName)</para>
            /// </summary>
            public string NickName;
            /// <summary>
            /// --
            /// <para>--</para>
            /// </summary>
            public int RankDes;
            /// <summary>
            /// 我的群成员备注
            /// <para>the Group Remark</para>
            /// </summary>
            public string Remark;
            /// <summary>
            /// 群成员最后一次发言时间
            /// <para>the last time he talkin</para>
            /// </summary>
            public int SpeakTime;
            /// <summary>
            /// 群成员当前状态 10在线 | 20隐身
            /// <para>the Status the Mebemer [int] for 10 is online and 20 is offline</para>
            /// </summary>
            public int Status;
            /// <summary>
            /// 是否群管理员 0F/1T
            /// <para>a parameter indecate this memeber is GroupAdministrator</para>
            /// </summary>
            public int GroupAdmin;
        }
    }
    /// <summary>
    /// 群列表
    /// <para>Group List class</para>
    /// </summary>
    public class QQGroupList
    {
        /// <summary>
        /// 群总数
        /// <para>the Total Count of QQGroup</para>
        /// </summary>
        public int Count;
        /// <summary>
        /// 拉取列表的下一个操作验证
        /// <para>the next gen of the Programme</para>
        /// </summary>
        public int? NextToken;
        /// <summary>
        /// 拉取的部分群列表
        /// <para>converted QQGroupList</para>
        /// </summary>
        public List<QQGroup> TroopList;
        /// <summary>
        /// QQ群的操作列表
        /// <para>Group Class</para>
        /// </summary>
        public class QQGroup
        {
            /// <summary>
            /// 群号
            /// <para>Group Number</para>
            /// </summary>
            public long GroupId;
            /// <summary>
            /// 群成员数
            /// <para>Group Member Count</para>
            /// </summary>
            public int GroupMemberCount;
            /// <summary>
            /// 群名
            /// <para>Group Name</para>
            /// </summary>
            public string GroupName;
            /// <summary>
            /// 群公告(获取最近一条置顶)
            /// <para>the Most Recently GroupAnnounce</para>
            /// </summary>
            public string GroupNotice;
            /// <summary>
            /// 群主
            /// <para>the Group Owner</para>
            /// </summary>
            public long GroupOwner;
            /// <summary>
            /// 群总成员数
            /// <para>Group Memeber Total Count</para>
            /// </summary>
            public int GroupTotalCount;
        }
        /// <summary>
        /// 内套的群列表
        /// <para>Inner class of Group List</para>
        /// </summary>
        public class _GroupList
        {
            /// <summary>
            /// 私有的群列表
            /// </summary>
            private List<QQGroup> q = new List<QQGroup>();
            /// <summary>
            /// 重构的对比添加
            /// </summary>
            /// <param name="n">项</param>
            /// <returns></returns>
            public bool Add(QQGroup n)
            {
                if (q.Exists((QQGroup k) => k.GroupId == n.GroupId))
                {
                    return false;
                }
                else
                {
                    q.Add(n);
                    return true;
                }
            }
            /// <summary>
            /// 获取群列表
            /// </summary>
            /// <returns></returns>
            public List<QQGroup> getInstance() => this.q;
        }
        /// <summary>
        /// 拉取群列表
        /// <para>the Group List</para>
        /// </summary>
        /// <param name="nt">
        /// 索引号
        /// <para>Index number</para>
        /// </param>
        /// <returns></returns>
        public static async Task<QQGroupList> GetGroupListOnIndex(int nt) =>
            JsonConvert.DeserializeObject<QQGroupList>(
                await PASA(
                    UrlType.GetGroupList,
                    $"{{\"NextToken\"{nt}}}")
                );
    }
    /// <summary>
    /// QQ群请求(管理类)方法
    /// <para>QQGroup Manager Method</para>
    /// </summary>
    public class QQGroupRequest
    {
        /// <summary>
        /// 总方法
        /// <para>General Method</para>
        /// </summary>
        /// <param name="ActionType">
        /// 8拉人 1加入 2退出 3移出
        /// <para>for [int] 8:invite 1:joinin 2:leave 3:remove-someone</para>
        /// </param>
        /// <param name="GroupID">
        /// 群号
        /// <para>GroupNumber</para>
        /// </param>
        /// <param name="ActionUserID">
        /// QQ用户ID *加入退出不用写* 
        /// <para>the QQ number (*for enter and quit please leave this blanked)</para>
        /// </param>
        /// <param name="Content">
        /// 内容
        /// <para>the Content</para>
        /// </param>
        public static async Task<string> __GroupManage(int ActionType, long GroupID, long ActionUserID, string Content) => await
            PostHelper.PASA(
                PostHelper.UrlType.GroupMgr,
                $"{{\"ActionType\":{ActionType},\"GroupID\":{GroupID}," +
                $"\"ActionUserID\":{ActionUserID},\"Content\":\"{Content}\"}}"
                );
    }
    /// <summary>
    /// QQ请求应答
    /// <para>QQ Action response</para>
    /// <para>请求应答已经禁用,以后的请求应答会在请求的委托中决定,保证请求的操作同步</para>
    /// <para>for request response is now OBSOLETE you need to go Event and set the Parameter</para>
    /// </summary>
    [Obsolete]
    public class QQRequestResponse
    {
        /// <summary>
        /// 处理好友添加
        /// <para>dealing with Friend Addition</para>
        /// </summary>
        /// <param name="Action"> 
        /// 1忽略 2同意 3拒绝 
        /// <para>for a type [int] 1:Ignore 2:Agree 3:Reject</para>
        /// </param>

        public static async Task<string> DealFriend(int Action) => await PASA(UrlType.DealFriend, $"{{\"Action\":{Action}}}");
        /// <summary>
        /// 处理群添加
        /// <para>dealing with Group Addition</para>
        /// </summary>
        /// <param name="Action"> 
        /// 11 同意 14 忽略 21 不同意 
        /// <para>for a type [int] 11:Agree 14:Ignore 21:Reject</para>
        /// </param>
        public static async Task<string> DealGroupInvite(int Action) => await PASA(UrlType.AnswerInviteGroup, $"{{\"Action\":{Action}}}");
    }
}


