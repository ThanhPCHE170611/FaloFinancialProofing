# FaloFinancialProofing
Final Capstone FPT


Quy trình pull, push, fetch:
Có thể sử dụng source tree để quản lý sử dụng git

nhánh để code chung: dev (sẽ được tạo sau)
mọi người khi pull về sẽ pull từ dev, sẽ fetch trước sau đó pull

pull về rồi tạo 1 branch mới từ branch đó bằng lệnh checkout "new_branch"

"new_branch" ở đây là tên branch mới, sẽ tạo theo feature hoặc task đang làm, ví dụ như là signup_feature

sau khi code xong thì sẽ push lên cái branch đang làm feature chứ không phải branch gốc(dev), ở đây đang ví dụ là signup_feature, fetch trước sau đó push

sau đấy lên github tạo pull request, chú ý là sẽ merge vào branch gốc, và ở đây là branch dev.

NÊÚ: có conflict, pull lại bản vừa push lên và vào vscode xanh để resolved, nếu vẫn không resolved được thì liên hệ cho thanhpc

Không có conflict và hiện tick xanh là thành công, không tự ý merge mà đợi để được merge request.

Trước khi push: fetch lại code origin và pull để lấy code mới nhất, trường hợp không có pull thì đã là code mới nhất rồi, bỏ qua,

Nếu như có code pull thì pull mới nhất về và merge vào brach hiện tại, nếu có conflict thì phải fix đến khi các chức năng của app
chạy bình thường, sau đấy push lên nhánh bản thân và tạo merge request vào nhánh chung(dev)
